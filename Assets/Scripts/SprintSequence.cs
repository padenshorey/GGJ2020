﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintSequence : MonoBehaviour
{
    private const float TIME_BETWEEN_MASHES = 1f;

    public Text inputText;
    public Text totalText;

    private int _teamId;
    private int _currentSprintSequenceIndex = 0;
    private List<SprintInput> _sprintInputSequence = new List<SprintInput>();
    private bool _canMash = false;

    //private ButtonIcon 

    public ButtonIcon buttonIcon;

    public delegate void EventHandler(int team);
    public event EventHandler OnSprintComplete;

    private AudioManager audioManager;

    private int buttonsLeft = 5;

    public void Setup(SprintData sprintData, int team)
    {
        audioManager = FindObjectOfType<AudioManager>();

        _teamId = team;
        GenerateSprintInputs(sprintData);
        totalText.text = "x " + sprintData.numberOfPressesRequired.ToString();
        StartSprint();
    }

    private void StartSprint()
    {
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

        StartCoroutine(ActivateUserInput());
    }

    private void Update()
    {
        if(_canMash)
        {
            CheckCurrentInput();
        }
    }

    private void GenerateSprintInputs(SprintData sprintData)
    {
        for (int i = 0; i < sprintData.numberOfButtons; i++)
        {
            _sprintInputSequence.Add(new SprintInput(GetRandomInput(), sprintData.numberOfPressesRequired));
        }
    }

    private string GetRandomInput()
    {
        return Enums.RepairButtons[Random.Range(0, Enums.RepairButtons.Length)];
    }

    private void CheckCurrentInput()
    {
        
        foreach(XboxController controller in GameManager.instance.GetTeamControllers(_teamId))
        {

            if(Input.GetButtonDown(_sprintInputSequence[_currentSprintSequenceIndex].ButtonToMash + controller.controllerId))
            {
                GameObject myRobot;
                
                if (_teamId == 1) {
                    myRobot= GameManager.instance.robot1;
                    GameManager.instance.blueSpeedLineManager.ShowSpeedLine();
                } else {
                    myRobot = GameManager.instance.robot2;
                    GameManager.instance.redSpeedLineManager.ShowSpeedLine();
                }

                myRobot.GetComponent<SpriteRenderer>().sprite = myRobot.GetComponent<SpriteRenderer>().sprite == myRobot.GetComponent<RobotStates>().robotRun2 ?  myRobot.GetComponent<RobotStates>().robotRun1 :  myRobot.GetComponent<RobotStates>().robotRun2;

                audioManager.PlaySound("steps");

                _sprintInputSequence[_currentSprintSequenceIndex].ButtonPressCount++;

                //int poop = _sprintInputSequence[_currentSprintSequenceIndex].ButtonPressTarget;                

                buttonsLeft--;
                totalText.text = "x " + buttonsLeft;

                if (_sprintInputSequence[_currentSprintSequenceIndex].ButtonPressCount >= _sprintInputSequence[_currentSprintSequenceIndex].ButtonPressTarget)
                {
                    OnStepComplete();
                    return;
                }
            }
        }
    }

    private void OnStepComplete()
    {
        _currentSprintSequenceIndex++;

        GameManager.instance.blueSpeedLineManager.HideSpeedLines();
        GameManager.instance.redSpeedLineManager.HideSpeedLines();

        if (_currentSprintSequenceIndex >= _sprintInputSequence.Count)
        {
            EndSprint();
            return;
        }

        StartCoroutine(ActivateUserInput());
    }

    IEnumerator ActivateUserInput()
    {
        //disable it all for 2 seconds
        _canMash = false;
        //totalText.text = "0";

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(TIME_BETWEEN_MASHES);

        // re-enable it all
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        buttonIcon.SetImage(_sprintInputSequence[_currentSprintSequenceIndex].ButtonToMash);

        inputText.text = _sprintInputSequence[_currentSprintSequenceIndex].ButtonToMash;
        buttonsLeft = _sprintInputSequence[_currentSprintSequenceIndex].ButtonPressTarget;
        totalText.text = "x " + buttonsLeft.ToString();
        _canMash = true;
    }

    private void EndSprint()
    {
        //Debug.Log("Ending Sprint for Team " + _teamId);        

        OnSprintComplete(_teamId);
        Destroy(this.gameObject);
    }
}
