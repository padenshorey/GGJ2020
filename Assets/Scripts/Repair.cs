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

    public Dictionary<string, Enums.RepairType> repairRequirements = new Dictionary<string, Enums.RepairType>();

    public Repair()
    {
        GenerateRepairRequirements();
    }

    public bool CheckForCompletion(XboxController controller)
    {
        foreach (KeyValuePair<string, Enums.RepairType> keyValuePair in repairRequirements)
        {
            if (!Input.GetButtonDown(keyValuePair.Key + controller.controllerId))
            {
                _isComplete =  false;
                return _isComplete;
            }
        }

        _isComplete = true;
        return _isComplete;
    }
    
    private void GenerateRepairRequirements()
    {
        int typeOfRepair = Random.Range(0, 10);

        if(typeOfRepair < 5)
        {
            //button press
            repairRequirements.Add(Enums.RepairButtons[(int)Random.Range(0, Enums.RepairButtons.Length)], Enums.RepairType.ButtonPress);
        }
        else
        {
            // button combo press
            string firstButton = Enums.RepairButtons[(int)Random.Range(0, Enums.RepairButtons.Length)];
            string secondButton = Enums.RepairButtons[(int)Random.Range(0, Enums.RepairButtons.Length)];

            while (firstButton == secondButton)
            {
                secondButton = Enums.RepairButtons[(int)Random.Range(0, Enums.RepairButtons.Length)];
            }

            repairRequirements.Add(firstButton, Enums.RepairType.ComboButtonPress);
            repairRequirements.Add(secondButton, Enums.RepairType.ComboButtonPress);
        }

        foreach(KeyValuePair<string, Enums.RepairType> keyValuePair in repairRequirements)
        {
            Debug.Log("Repair Created: " + keyValuePair.Key + ", " + keyValuePair.Value);
        }
    }
}
