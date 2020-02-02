using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

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

    public enum Cutscene
    {
        Repair = 0,
        Broken = 1,
        Intro = 2,
        EndGame = 3
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

    public static string[] RepairStickDirections =
    {
        "Up",
        "Down",
        "Left",
        "Right"
    };

    public static Dictionary<string, float[]> StickRepairDirectionValues = new Dictionary<string, float[]>()
    {
        {"Up", new float[]{0, 1} },
        {"Down", new float[]{0, -1} },
        {"Left", new float[]{-1, 0} },
        {"Right", new float[]{1, 0} }
    };
}
