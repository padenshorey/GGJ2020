using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SprintData", menuName = "ScriptableObjects/SprintData", order = 2)]
public class SprintData : ScriptableObject
{
    public int numberOfPressesRequired;
    public int numberOfButtons;
}
