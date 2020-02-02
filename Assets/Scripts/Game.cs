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

    //public AudioManager audioManager;
    public PlayerController player;

    private List<Round> _team1CompletedRounds = new List<Round>();
    private List<Round> _team2CompletedRounds = new List<Round>();
    public Game(int roundCount)
    {
        _roundCount = roundCount;
        StartGame();
    }

    public void StartGame()
    {

        GameObject[] checkmarks = GameObject.FindGameObjectsWithTag("ReadyCheckmark");
        
        for(int i = 0; i < checkmarks.Length; i ++)
        {
            Debug.Log("ReadyCheckmark" + i);
            checkmarks[i].SetActive(false);
        }

        StartNextSprintSequence(1);
        StartNextSprintSequence(2);
    }

    public void StartNextSprintSequence(int team)
    {
        if (team == 1)
        {
            GameManager.instance.craneLeft.SetActive(false);
            GameManager.instance.playersSpawner01.SetActive(false);
            GameManager.instance.playersSpawner02.SetActive(false);
        } else 
        {
            GameManager.instance.craneRight.SetActive(false);
            GameManager.instance.playersSpawner03.SetActive(false);
            GameManager.instance.playersSpawner04.SetActive(false);
        }

        GameObject sprintPrefab = Resources.Load("SprintSequence") as GameObject;
        GameObject sprint = GameObject.Instantiate(sprintPrefab, GameManager.instance.GameCanvas.transform.Find("Team" + team.ToString()));
        SprintSequence currentSprintSequence = sprint.GetComponent<SprintSequence>();
        currentSprintSequence.Setup(GameManager.instance.SprintData[team == 1 ? _currentSprintTeam1 : _currentSprintTeam2], team);
        currentSprintSequence.OnSprintComplete += OnSprintComplete;
    }

    private void OnSprintComplete(int team)
    {
        //Debug.Log("On Sprint Complete Team " + team.ToString());
        switch (team)
        {
            case 1:
                _currentSprintTeam1++;

                
                GameManager.instance.craneLeft.SetActive(true);
                GameManager.instance.playersSpawner01.SetActive(true);
                GameManager.instance.playersSpawner02.SetActive(true);

                if (_currentSprintTeam1 >= GameManager.instance.SprintData.Length)
                {
                    EndGame(1);
                    return;
                }
                GameManager.instance.CutsceneManager.SpawnCutscene(1, Enums.Cutscene.Broken);
                _currentRoundTeam1 = StartRound(_team1CompletedRounds.Count+1, 1);
                break;
            case 2:
                _currentSprintTeam2++;


                GameManager.instance.craneRight.SetActive(true);
                GameManager.instance.playersSpawner03.SetActive(true);
                GameManager.instance.playersSpawner04.SetActive(true);

                Debug.Log("_currentSprintTeam2 set to " + _currentSprintTeam2);

                if (_currentSprintTeam2 >= GameManager.instance.SprintData.Length)
                {
                    EndGame(2);
                    return;
                }
                GameManager.instance.CutsceneManager.SpawnCutscene(2, Enums.Cutscene.Broken);
                _currentRoundTeam2 = StartRound(_team2CompletedRounds.Count+1, 2);
                break;
            default:
                break;
        }
    }

    public void EndGame(int winningTeamId)
    {
        //TODO: Handle end game animations (or whatever we want to happen).
        Debug.Log("TEAM " + winningTeamId + " WINS!");

        GameManager.instance.EndGame(winningTeamId);

        foreach(PlayerController pc in GameManager.instance.team1)
        {
            pc.enabled = false;
        }

        foreach(PlayerController pc in GameManager.instance.team2)
        {
            pc.enabled = false;
        }
    }

    public void CheckRoundsComplete(int team)
    {
        if (team == 1 )
        {
            _team1CompletedRounds.Add(_currentRoundTeam1);
            HideRepairAvatarsForTeam(1);
            GameManager.instance.CutsceneManager.SpawnCutscene(1, Enums.Cutscene.Repair);
            GameManager.instance.craneLeft.SetActive(false);
            GameManager.instance.playersSpawner01.SetActive(false);
            GameManager.instance.playersSpawner02.SetActive(false);
            StartNextSprintSequence(1);

            GameObject myRobot = GameManager.instance.robot1;
            myRobot.GetComponent<SpriteRenderer>().sprite = myRobot.GetComponent<RobotStates>().robotGood; 
            
        }else
        {
            GameManager.instance.CutsceneManager.SpawnCutscene(2, Enums.Cutscene.Repair);

            HideRepairAvatarsForTeam(2);

            GameManager.instance.craneRight.SetActive(false);
            GameManager.instance.playersSpawner03.SetActive(false);
            GameManager.instance.playersSpawner04.SetActive(false);

            _team2CompletedRounds.Add(_currentRoundTeam2);
            StartNextSprintSequence(2);

            GameObject myRobot = GameManager.instance.robot2;
            myRobot.GetComponent<SpriteRenderer>().sprite = myRobot.GetComponent<RobotStates>().robotGood; 
        }
    }

    private void HideRepairAvatarsForTeam(int team)
    {
        if(team == 1)
        {
            foreach(PlayerController pc in GameManager.instance.team1)
            {
                pc.canvasPlayer.SetActive(false);
            }
        }
        else
        {
            foreach (PlayerController pc in GameManager.instance.team2)
            {
                pc.canvasPlayer.SetActive(false);
            }
        }
    }

    public Round StartRound(int roundNumber, int teamId)
    {
        
        if (teamId == 1) {
            GameManager.instance.craneLeft.SetActive(true);
            GameManager.instance.playersSpawner01.SetActive(true);
            GameManager.instance.playersSpawner02.SetActive(true);
        } else {
            GameManager.instance.craneRight.SetActive(true);
            GameManager.instance.playersSpawner03.SetActive(true);
            GameManager.instance.playersSpawner04.SetActive(true);
        }

        Round currentRound = new Round(roundNumber, teamId, GameManager.instance.RoundData[roundNumber - 1].RoundDuration, GameManager.instance.RoundData[roundNumber - 1].InstructionCardCount);


        if (currentRound.RoundNumber >= 1) {
            if (teamId == 1) {
            GameObject myRobot = GameManager.instance.robot1;
            myRobot.GetComponent<SpriteRenderer>().sprite = myRobot.GetComponent<RobotStates>().robotBad;
            } else {
                GameObject myRobot = GameManager.instance.robot2;

                 myRobot.GetComponent<SpriteRenderer>().sprite = myRobot.GetComponent<RobotStates>().robotBad;
            }
        }

        currentRound.OnRoundComplete += CheckRoundsComplete;
        
        return currentRound;
    }


}
