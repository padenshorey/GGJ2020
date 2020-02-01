using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : MonoBehaviour
{
    // button press
    // button combo press
    // right stick movements

    private bool _isComplete = false;
    public bool IsComplete { get { return _isComplete; } }

    private Enums.RepairType _repairType;

    public Dictionary<string, Enums.RepairType> repairRequirements = new Dictionary<string, Enums.RepairType>();

    public Repair()
    {
        GenerateRepairRequirements();
    }

    public bool CheckForCompletion(XboxController controller)
    {
        foreach (KeyValuePair<string, Enums.RepairType> keyValuePair in repairRequirements)
        {
            switch(keyValuePair.Value)
            {
                case Enums.RepairType.StickDirection:
                    if ((Input.GetAxis(controller.joyRightHori) - Enums.StickRepairDirectionValues[keyValuePair.Key][0]) > 0.1f &&
                        (Input.GetAxis(controller.joyRightVert) - Enums.StickRepairDirectionValues[keyValuePair.Key][1]) > 0.1f)
                    {
                        _isComplete = false;
                        return _isComplete;
                    }
                    break;
                default:
                    if (!Input.GetButtonDown(keyValuePair.Key + controller.controllerId))
                    {
                        _isComplete = false;
                        return _isComplete;
                    }
                    break;
            }
        }

        _isComplete = true;
        return _isComplete;
    }
    
    private void GenerateRepairRequirements()
    {
        int typeOfRepair = Random.Range(0, 10);

        if(typeOfRepair < 8)
        {
            //button press
            _repairType = Enums.RepairType.ButtonPress;
            repairRequirements.Add(Enums.RepairButtons[(int)Random.Range(0, Enums.RepairButtons.Length)], _repairType);
        }
        else if(typeOfRepair < 11)
        {
            // button combo press
            _repairType = Enums.RepairType.ComboButtonPress;
            string firstButton = Enums.RepairButtons[(int)Random.Range(0, Enums.RepairButtons.Length)];
            string secondButton = Enums.RepairButtons[(int)Random.Range(0, Enums.RepairButtons.Length)];

            while (firstButton == secondButton)
            {
                secondButton = Enums.RepairButtons[(int)Random.Range(0, Enums.RepairButtons.Length)];
            }

            repairRequirements.Add(firstButton, _repairType);
            repairRequirements.Add(secondButton, _repairType);
        }
        else
        {
            _repairType = Enums.RepairType.StickDirection;
            repairRequirements.Add(Enums.RepairStickDirections[(int)Random.Range(0, Enums.RepairStickDirections.Length)], _repairType);
        }

        foreach(KeyValuePair<string, Enums.RepairType> keyValuePair in repairRequirements)
        {
            Debug.Log("Repair Created: " + keyValuePair.Key + ", " + keyValuePair.Value);
        }
    }
}
