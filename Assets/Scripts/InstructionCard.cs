using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionCard : MonoBehaviour
{
    private List<Repair> _repairs = new List<Repair>();
    private List<string> repairNames = new List<string>();
    public GameObject instructionName;
    private int _repairCount;
    private int _currentRepairStep = 0;
    private bool _isSelected = false;
    private Enums.InstructionType _repairType;
    private bool _isComplete = false;
    public bool IsComplete { get { return _isComplete; } }

    public delegate void EventHandler();
    public event EventHandler OnCardComplete;

    private List<XboxController> teamControllers = new List<XboxController>();

    void Update()
    {
        //if(_isSelected)
        //{
            if(!_isComplete) CheckForInput();
        //} 
    }

    private void FinishInstructionCard()
    {
        // is done
        Debug.Log("Finished Instruction Card");
        _isComplete = true;
        OnCardComplete();
    }

    private void CheckForInput()
    {
        // make sure the input is done in sequence according to the repair order
        bool repairComplete = false;
        //Debug.Log("Check for input");
        foreach(XboxController controller in teamControllers)
        {
            repairComplete = _repairs[_currentRepairStep].CheckForCompletion(controller);
            if (repairComplete) break;
        }

        if(repairComplete)
        {
            string toDebug = "Repair Complete: ";
            foreach(KeyValuePair<string, Enums.RepairType> keyValuePair in _repairs[_currentRepairStep].repairRequirements)
            {
                toDebug += keyValuePair.Key;
                toDebug += " ";
            }
            

            Debug.Log("Repair Complete: " + _repairs[_currentRepairStep].repairRequirements);
            if(AllRepairsComplete())
            {
                FinishInstructionCard();
            }
            else
            {
                _currentRepairStep++;
            }
        }
    }

    public void Setup(Enums.InstructionType repairType, int repairCount, int teamId)
    {
        Debug.Log(repairType.ToString() + " instruction card with " + repairCount + " repairs created.");

        teamControllers = GameManager.instance.GetTeamControllers(teamId);
        _repairType = repairType;
        _repairCount = repairCount;
        GenerateRepairs();
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
