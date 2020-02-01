using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private int _roundCount;
    private Round _currentRoundTeam1;
    private Round _currentRoundTeam2;
    public void Setup(int roundCount)
    {
        _roundCount = roundCount;

        StartGame();
    }

    public void StartGame()
    {
        _currentRoundTeam1 = StartRound(1, 1);
        _currentRoundTeam2 = StartRound(1, 2);
    }

    public Round StartRound(int roundNumber, int teamId)
    {
        Round currentRound = new Round(roundNumber, teamId, GameManager.instance.RoundData[roundNumber - 1].RoundDuration, GameManager.instance.RoundData[roundNumber - 1].InstructionCardCount);
        return currentRound;
    }
}
