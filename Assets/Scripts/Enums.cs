using UnityEngine;

public class Enums 
{
    public enum InstructionType
    {
        Solo = 0,
        Team = 1
    }

    public enum RepairType
    {
        ButtonPress = 0,
        ComboButtonPress = 1,
        StickDirection = 2
    }

    public static string[] RepairButtons =
    {
        "A_",
        "B_",
        "X_",
        "Y_",
        "LB_",
        "RB_"
    };
}
