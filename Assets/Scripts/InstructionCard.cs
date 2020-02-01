using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionCard : MonoBehaviour
{
    private const float INSTRUCTION_CARD_INPUT_DELAY = 0.5f;

    private List<Repair> _repairs = new List<Repair>();
    private List<string> repairNames = new List<string>();
    public GameObject instructionName;
    private int _repairCount;
    private int _currentRepairStep = 0;
    private bool _isSelected = false;
    private Enums.InstructionType _instructionType;
    private bool _isComplete = false;
    private bool _canRepair = false;
    public bool IsComplete { get { return _isComplete; } }

    public delegate void EventHandler();
    public event EventHandler OnCardComplete;

    private List<XboxController> teamControllers = new List<XboxController>();

    void Update()
    {
        //if(_isSelected)
        //{
            if(!_isComplete && _canRepair) CheckForInput();
        //} 
    }

    private void FinishInstructionCard()
    {
        Debug.Log("Finished Instruction Card");
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
                repairComplete = _repairs[_currentRepairStep].CheckForCompletion(controller);
                if (repairComplete) break;
            }
        }
        else
        {
            repairComplete = _repairs[_currentRepairStep].CheckForCompletion(teamControllers[0], teamControllers[1]);
        }

        // if the repair is complete, continue to the next repair or finish the instruction card
        if(repairComplete)
        {
            /*
            string toDebug = "Repair Complete: ";
            foreach(KeyValuePair<string, Enums.RepairType> keyValuePair in _repairs[_currentRepairStep].repairRequirements)
            {
                toDebug += keyValuePair.Key;
                toDebug += " ";
            }*/
            

            Debug.Log("Repair Complete: " + _repairs[_currentRepairStep].repairRequirements);
            if(AllRepairsComplete())
            {
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

    public void Setup(Enums.InstructionType repairType, int repairCount, int teamId)
    {
        Debug.Log(repairType.ToString() + " instruction card with " + repairCount + " repairs created.");

        teamControllers = GameManager.instance.GetTeamControllers(teamId);
        _instructionType = repairType;
        _repairCount = repairCount;
        GenerateRepairs();

        StartCoroutine(ActivateUserInput());
    }

    IEnumerator ActivateUserInput()
    {
        yield return new WaitForSeconds(INSTRUCTION_CARD_INPUT_DELAY);
        _canRepair = true;
    }

    public void GenerateRepairs()
    {
        repairNames.Add("Fix the thingie");
        repairNames.Add("turn the poop stick");
        repairNames.Add("Ignite the flames of war");
        repairNames.Add("Crank the shaft");
        repairNames.Add("Dial in the finnagle");

        
        //Debug.Log(instructionName.GetComponent<Text>().text);

        for (int i = 0; i < _repairCount; i++)
        {
            instructionName.GetComponent<Text>().text = repairNames[Random.Range(0,repairNames.Count)];

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
