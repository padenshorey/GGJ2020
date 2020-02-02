using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair 
{
    private const float REPAIR_BUTTON_PRESS_WEIGHT = 10f;
    private const float REPAIR_BUTTON_COMBO_WEIGHT = 0f;
    private const float REPAIR_STICK_DIRECTION_WEIGHT = 10f;

    private bool _isComplete = false;
    public bool IsComplete { get { return _isComplete; } }


    private Enums.RepairType _repairType;

    public Dictionary<string, Enums.RepairType> repairRequirements = new Dictionary<string, Enums.RepairType>();

    public Repair()
    {
        GenerateRepairRequirements();
    }

    public bool CheckForCompletion(XboxController controller1, XboxController controller2 = null)
    {
        _isComplete = true;

        //Debug.Log("Checking Controller " + controller1.controllerId + " for completion of " + _repairType);

        foreach (KeyValuePair<string, Enums.RepairType> keyValuePair in repairRequirements)
        {

            if (keyValuePair.Value == Enums.RepairType.StickDirection)
            {
                if (Input.GetAxisRaw(controller1.dpadHori) == Enums.StickRepairDirectionValues[keyValuePair.Key][0] &&
                    Input.GetAxisRaw(controller1.dpadVert) == Enums.StickRepairDirectionValues[keyValuePair.Key][1])
                {
                    if (controller2 != null)
                    {
                        if (Input.GetAxisRaw(controller2.dpadHori) != Enums.StickRepairDirectionValues[keyValuePair.Key][0] ||
                            Input.GetAxisRaw(controller2.dpadVert) != Enums.StickRepairDirectionValues[keyValuePair.Key][1])
                        {
                            _isComplete = false;
                        }
                    }
                }
                else
                {
                    _isComplete = false;
                }
            }
            else
            {
                if (Input.GetButtonDown(keyValuePair.Key + controller1.controllerId))
                {
                    if (controller2 != null)
                    {
                        if (!Input.GetButtonDown(keyValuePair.Key + controller2.controllerId))
                        {
                            _isComplete = false;
                        }
                    }
                }
                else
                {
                    _isComplete = false;
                }
            }
        }

        return _isComplete;
    }

    private void GenerateRepairRequirements()
    {
        float typeOfRepair = Random.Range(0f, 1f);
        float totalWeight = REPAIR_BUTTON_PRESS_WEIGHT + REPAIR_BUTTON_COMBO_WEIGHT + REPAIR_STICK_DIRECTION_WEIGHT;

        if (typeOfRepair < (REPAIR_BUTTON_PRESS_WEIGHT/totalWeight))
        {
            //button press
            _repairType = Enums.RepairType.ButtonPress;
            repairRequirements.Add(Enums.RepairButtons[(int)Random.Range(0, Enums.RepairButtons.Length)], _repairType);
        }
        else if(typeOfRepair < ((REPAIR_BUTTON_PRESS_WEIGHT + REPAIR_BUTTON_COMBO_WEIGHT)/ totalWeight))
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
            //Debug.Log("Repair Created: " + keyValuePair.Key + ", " + keyValuePair.Value);
        }
    }
}
