using System;
using UnityEngine;
using static Enums;

public class Round 
{
    private const int TEAM_INSTRUCTION_CARD_PROBABILITY = 10;

    private int _teamId;
    private float _roundDuration;
    private int _instructionCardCount;
    private int _roundNumber;
    public int RoundNumber { get { return _roundNumber; } }
    private bool _roundStarted = false;
    private bool _roundComplete = false;
    public bool RoundComplete { get { return _roundComplete; } }
    private System.Collections.Generic.List<InstructionCard> _intructions = new System.Collections.Generic.List<InstructionCard>();

    public delegate void EventHandler(int team);
    public event EventHandler OnRoundComplete;

    public Round(int roundNumber, int teamID, float duration, int instructionCardCount)
    {
        _roundNumber = roundNumber;
        _teamId = teamID;
        _roundDuration = duration;
        _instructionCardCount = instructionCardCount;

        ////Debug.Log("Round " + _roundNumber + " for Team " + teamID + " with " + _instructionCardCount + " instructions created.");

        GenerateInstructionCards(_instructionCardCount);

        //TODO: Maybe add a countdown or something before starting the round?
        StartRound();
    }

    public void StartRound()
    {
        _roundStarted = true;
        GameManager.instance.AssignPlayersToInstructionFrame(_teamId, _intructions);

        if (_teamId == 1)
        {
            GameManager.instance.team1CurrentRound++;
            GameManager.instance.drone.SetPosition(1, GameManager.instance.team1CurrentRound, GameManager.instance.TotalRounds);
        }
        else
        {
            GameManager.instance.team2CurrentRound++;
            GameManager.instance.drone.SetPosition(2, GameManager.instance.team2CurrentRound, GameManager.instance.TotalRounds);
        }


        ////Debug.Log("Starting round " + _roundNumber + " for Team " + _teamId);
    }

    private void EndRound()
    {
        ////Debug.Log("Ending round " + _roundNumber + " for Team " + _teamId);


        foreach(InstructionCard ic in _intructions)
        {
            ic.currentInstructionCard.DisableCard();
        }

        _roundComplete = true;
        OnRoundComplete(_teamId);

        //GameManager.instance.playersSpawner.SetActive(false);
    }

    private void CheckRoundComplete()
    {
        ////Debug.Log("Check Round Complete");

        foreach (InstructionCard ic in _intructions)
        {
            if (!ic.IsComplete) return;
        }

        EndRound();
    }

    public void GenerateInstructionCards(int cardCount)
    {
        for(int i = 0; i < cardCount; i++)
        {
            GameObject cardPrefab = Resources.Load("InstructionCard") as GameObject;
            GameObject card = GameObject.Instantiate(cardPrefab, GameManager.instance.GameCanvas.transform);
            InstructionCard currentInstructionCard = card.GetComponent<InstructionCard>();
            currentInstructionCard.Setup(GetRepairType(), (int)UnityEngine.Random.Range(GameManager.instance.RoundData[_roundNumber - 1].MinRepairCount, GameManager.instance.RoundData[_roundNumber - 1].MaxRepairCount), _teamId);
            currentInstructionCard.OnCardComplete += CheckRoundComplete;
            _intructions.Add(currentInstructionCard);
        }
    }

    private InstructionType GetRepairType()
    {
        // make sure no team instructions are given to solo teams
        if(_teamId == 1)
        {
            if (GameManager.instance.Team1Count < 2) return InstructionType.Solo;
        }
        else
        {
            if (GameManager.instance.Team2Count < 2) return InstructionType.Solo;
        }

        //TODO: Figure out how to decide this
        int randomNum = (int)UnityEngine.Random.Range(0f, 100f);
        if (randomNum > TEAM_INSTRUCTION_CARD_PROBABILITY) return InstructionType.Solo;
        else return InstructionType.Team;
    }
}
