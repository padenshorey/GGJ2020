using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game 
{
    private int _roundCount;
    private Round _currentRoundTeam1;
    private Round _currentRoundTeam2;

    private List<Round> _team1CompletedRounds = new List<Round>();
    private List<Round> _team2CompletedRounds = new List<Round>();
    public Game(int roundCount)
    {
        _roundCount = roundCount;
        StartGame();
    }

    public void StartGame()
    {
        _currentRoundTeam1 = StartRound(1, 1);
        _currentRoundTeam2 = StartRound(1, 2);
    }

    public void EndGame(int winningTeamId)
    {
        //TODO: Handle Game End
        Debug.Log("TEAM " + winningTeamId + " WINS!");
    }

    public void CheckRoundsComplete()
    {
        if (_currentRoundTeam1.RoundComplete)
        {
            _team1CompletedRounds.Add(_currentRoundTeam1);
            if (_currentRoundTeam1.RoundNumber == _roundCount)
            {
                EndGame(1);
            }
            else
            {
                _currentRoundTeam1 = StartRound(_currentRoundTeam1.RoundNumber + 1, 1);
            }
        }
        else if (_currentRoundTeam2.RoundComplete)
        {
            _team2CompletedRounds.Add(_currentRoundTeam2);
            if (_currentRoundTeam2.RoundNumber == _roundCount)
            {
                EndGame(2);
            }
            else
            {
                _currentRoundTeam2 = StartRound(_currentRoundTeam2.RoundNumber + 1, 2);
            }
        }
    }

    public Round StartRound(int roundNumber, int teamId)
    {
        Round currentRound = new Round(roundNumber, teamId, GameManager.instance.RoundData[roundNumber - 1].RoundDuration, GameManager.instance.RoundData[roundNumber - 1].InstructionCardCount);

        currentRound.OnRoundComplete += CheckRoundsComplete;
        
        return currentRound;
    }


}
