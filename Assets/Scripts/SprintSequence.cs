using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintSequence : MonoBehaviour
{
    private const float TIME_BETWEEN_MASHES = 2f;

    public Text inputText;
    public Text totalText;

    private int _teamId;
    private int _currentSprintSequenceIndex = 0;
    private List<SprintInput> _sprintInputSequence = new List<SprintInput>();
    private bool _canMash = false;

    public delegate void EventHandler(int team);
    public event EventHandler OnSprintComplete;

    private AudioManager audioManager;

    public void Setup(SprintData sprintData, int team)
    {
        audioManager = FindObjectOfType<AudioManager>();

        _teamId = team;
        GenerateSprintInputs(sprintData);
        StartSprint();
    }

    private void StartSprint()
    {
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
                Debug.Log("YOO");

                audioManager.PlaySound("steps");

                _sprintInputSequence[_currentSprintSequenceIndex].ButtonPressCount++;
                totalText.text = _sprintInputSequence[_currentSprintSequenceIndex].ButtonPressCount.ToString();
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
        if(_currentSprintSequenceIndex >= _sprintInputSequence.Count)
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
        totalText.text = "0";
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
        inputText.text = _sprintInputSequence[_currentSprintSequenceIndex].ButtonToMash;
        _canMash = true;
    }

    private void EndSprint()
    {
        Debug.Log("Ending Sprint for Team " + _teamId);
        OnSprintComplete(_teamId);
        Destroy(this.gameObject);
    }
}
