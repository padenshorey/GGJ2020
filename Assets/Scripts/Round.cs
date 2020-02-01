using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class Round : MonoBehaviour
{
    private int _teamId;
    private float _roundDuration;
    private int _instructionCardCount;
    private int _roundNumber;
    private bool _roundStarted = false;
    private bool _roundComplete = false;
    private List<InstructionCard> _intructions = new List<InstructionCard>();

    public Round(int roundNumber, int teamID, float duration, int instructionCardCount)
    {
        _roundNumber = roundNumber;
        _teamId = teamID;
        _roundDuration = duration;
        _instructionCardCount = instructionCardCount;

        GenerateInstructionCards(_instructionCardCount);
    }

    public void GenerateInstructionCards(int cardCount)
    {
        for(int i = 0; i < cardCount; i++)
        {
            _intructions.Add(new InstructionCard(GetRepairType(), (int)Random.Range(GameManager.instance.RoundData[_roundNumber-1].MinRepairCount, GameManager.instance.RoundData[_roundNumber - 1].MaxRepairCount)));
        }
    }

    private RepairType GetRepairType()
    {//TODO: Figure out how to decide this
        return (RepairType)((int)Random.Range(0f, 1f));
    }
}
