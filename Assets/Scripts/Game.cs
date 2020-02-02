using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game 
{
    private int _roundCount;
    private Round _currentRoundTeam1;
    private Round _currentRoundTeam2;

    private int _currentSprintTeam1 = 0;
    private int _currentSprintTeam2 = 0;

    private List<Round> _team1CompletedRounds = new List<Round>();
    private List<Round> _team2CompletedRounds = new List<Round>();
    public Game(int roundCount)
    {
        _roundCount = roundCount;
        StartGame();
    }

    public void StartGame()
    {
        StartNextSprintSequence(1);
        StartNextSprintSequence(2);
    }

    public void StartNextSprintSequence(int team)
    {
        GameObject sprintPrefab = Resources.Load("SprintSequence") as GameObject;
        GameObject sprint = GameObject.Instantiate(sprintPrefab, GameManager.instance.GameCanvas.transform.Find("Team" + team.ToString()));
        SprintSequence currentSprintSequence = sprint.GetComponent<SprintSequence>();
        currentSprintSequence.Setup(GameManager.instance.SprintData[team == 1 ? _currentSprintTeam1 : _currentSprintTeam2], team);
        currentSprintSequence.OnSprintComplete += OnSprintComplete;
    }

    private void OnSprintComplete(int team)
    {
        switch (team)
        {
            case 1:
                _currentSprintTeam1++;
                if (_currentSprintTeam1 >= GameManager.instance.SprintData.Length)
                {
                    EndGame(1);
                    return;
                }
                _currentRoundTeam1 = StartRound(_team1CompletedRounds.Count+1, 1);
                break;
            case 2:
                _currentSprintTeam2++;
                if (_currentSprintTeam2 >= GameManager.instance.SprintData.Length)
                {
                    EndGame(2);
                    return;
                }
                _currentRoundTeam2 = StartRound(_team2CompletedRounds.Count + 1, 2);
                break;
            default:
                break;
        }
    }

    public void EndGame(int winningTeamId)
    {
        //TODO: Handle end game animations (or whatever we want to happen).
        Debug.Log("TEAM " + winningTeamId + " WINS!");

        GameManager.instance.EndGame();
    }

    public void CheckRoundsComplete()
    {
        if (_currentRoundTeam1.RoundComplete)
        {
            _team1CompletedRounds.Add(_currentRoundTeam1);
            GameManager.instance.CutsceneManager.SpawnCutscene(1, Enums.Cutscene.Repair);
            StartNextSprintSequence(1);
        }
        else if (_currentRoundTeam2.RoundComplete)
        {
            GameManager.instance.CutsceneManager.SpawnCutscene(2, Enums.Cutscene.Repair);
            _team2CompletedRounds.Add(_currentRoundTeam2);
            StartNextSprintSequence(2);
        }
    }

    public Round StartRound(int roundNumber, int teamId)
    {
        Round currentRound = new Round(roundNumber, teamId, GameManager.instance.RoundData[roundNumber - 1].RoundDuration, GameManager.instance.RoundData[roundNumber - 1].InstructionCardCount);

        currentRound.OnRoundComplete += CheckRoundsComplete;
        
        return currentRound;
    }


}
