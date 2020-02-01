using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionCard : MonoBehaviour
{
    private List<Repair> _repairs = new List<Repair>();

    private int _repairCount;

    private bool _isSelected = false;

    private Enums.RepairType _repairType;

    void Update()
    {
        if(_isSelected)
        {
            CheckForInput();
            if(AllRepairsComplete())
            {
                FinishInstruction();
            }
        } 
    }

    private void FinishInstruction()
    {
        // is done
    }

    private void CheckForInput()
    {
        // make sure the input is done in sequence according to the repair order
    }

    public InstructionCard(Enums.RepairType repairType, int repairCount)
    {
        _repairType = repairType;
        _repairCount = repairCount;
        GenerateRepairs();
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
