using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundData", menuName = "ScriptableObjects/RoundData", order = 1)]
public class RoundData : ScriptableObject
{
    public float RoundDuration;
    public int InstructionCardCount;

    public int MinRepairCount;
    public int MaxRepairCount;
}
