﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : MonoBehaviour
{
    private const float REPAIR_BUTTON_PRESS_WEIGHT = 10f;
    private const float REPAIR_BUTTON_COMBO_WEIGHT = 2f;
    private const float REPAIR_STICK_DIRECTION_WEIGHT = 2f;

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
                    if ((Input.GetAxis(controller.joyRightHori) - Enums.StickRepairDirectionValues[keyValuePair.Key][0]) > 0.3f &&
                        (Input.GetAxis(controller.joyRightVert) - Enums.StickRepairDirectionValues[keyValuePair.Key][1]) > 0.3f)
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
            Debug.Log("Repair Created: " + keyValuePair.Key + ", " + keyValuePair.Value);
        }
    }
}
