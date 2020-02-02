using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionCard : MonoBehaviour
{
    private const float INSTRUCTION_CARD_INPUT_DELAY = 0.05f;

    private List<Repair> _repairs = new List<Repair>();
    public List<Repair> Repairs { get { return _repairs; } }
    private List<string> repairNames = new List<string>();
    private int _repairCount;
    private int _currentRepairStep = 0;
    public bool isSelected = false;
    private Enums.InstructionType _instructionType;
    public Enums.InstructionType InstructionType { get { return _instructionType; } }
    private bool _isComplete = false;
    private bool _canRepair = false;
    public bool IsComplete { get { return _isComplete; } }

    public delegate void EventHandler();
    public event EventHandler OnCardComplete;

    public InstructionCardFrame currentInstructionCard;

    private List<XboxController> teamControllers = new List<XboxController>();

    void Update()
    {
        if(isSelected)
        {
            if(!_isComplete && _canRepair) CheckForInput();
        } 
    }

    private void FinishInstructionCard()
    {
        //Debug.Log("Finished Instruction Card");
        _isComplete = true;
        OnCardComplete();
    }

    private void CheckForInput()
    {
        // make sure the input is done in sequence according to the repair order

        // check to see if the repair is complete
        bool repairComplete = false;

        if (_instructionType == Enums.InstructionType.Solo)
        {
            foreach (XboxController controller in teamControllers)
            {
                if (!repairComplete)
                {
                    foreach (PlayerController pc in _teamId == 1 ? GameManager.instance.team1 : GameManager.instance.team2)
                    {
                        if (controller.controllerId == pc.ControllerId &&
                            pc.currentSelectedColumn == currentInstructionCard.columnId &&
                            pc.currentSelectedRow == currentInstructionCard.rowId)
                        {
                            repairComplete = _repairs[_currentRepairStep].CheckForCompletion(controller);
                            if (repairComplete) break;
                        }
                    }
                }
            }
        }
        else
        {
            repairComplete = _repairs[_currentRepairStep].CheckForCompletion(teamControllers[0], teamControllers[1]);
        }

        // if the repair is complete, continue to the next repair or finish the instruction card
        if(repairComplete)
        {
            //Debug.Log("Repair Complete: " + _repairs[_currentRepairStep].repairRequirements);
            currentInstructionCard.IncreaseStep();
            if (AllRepairsComplete())
            {
                currentInstructionCard.SetAsComplete();
                FinishInstructionCard();
            }
            else
            {
                _canRepair = false;
                StartCoroutine(ActivateUserInput());
                _currentRepairStep++;
            }
        }
    }

    private int _teamId;

    public void Setup(Enums.InstructionType repairType, int repairCount, int teamId)
    {
        //Debug.Log(repairType.ToString() + " instruction card with " + repairCount + " repairs created.");

        _teamId = teamId;
        teamControllers = GameManager.instance.GetTeamControllers(teamId);
        _instructionType = repairType;
        _repairCount = repairCount;
        GenerateRepairs();
        AssignToPlaceholder();

        StartCoroutine(ActivateUserInput());
    }

    IEnumerator ActivateUserInput()
    {
        yield return new WaitForSeconds(INSTRUCTION_CARD_INPUT_DELAY);
        _canRepair = true;
    }

    public void AssignToPlaceholder()
    {
        do
        {
            currentInstructionCard = _teamId == 1 ? GameManager.instance.instructionCardFramesTeam1[Random.Range(0, GameManager.instance.instructionCardFramesTeam1.Length)] : GameManager.instance.instructionCardFramesTeam2[Random.Range(0, GameManager.instance.instructionCardFramesTeam2.Length)];
        } while (!currentInstructionCard || currentInstructionCard.InUse);

        currentInstructionCard.ActivateCard(this);
    }

    public void GenerateRepairs()
    {      
        for (int i = 0; i < _repairCount; i++)
        {
            _repairs.Add(new Repair());
        }
    }

    public bool AllRepairsComplete()
    {
        for (int i = 0; i < _repairs.Count; i++)
        {
            if (!_repairs[i].IsComplete) return false;
        }

        return true;
    }
}
