﻿using System.Collections.Generic;
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

    public static string[] RepairStickDirections =
    {
        "Up",
        "Down",
        "Left",
        "Right",
        "Up/Left",
        "Up/Right",
        "Down/Left",
        "Down/Right"
    };

    public static Dictionary<string, float[]> StickRepairDirectionValues = new Dictionary<string, float[]>()
    {
        {"Up", new float[]{0, -1} },
        {"Down", new float[]{0, 1} },
        {"Left", new float[]{-1, 0} },
        {"Right", new float[]{1, 0} },
        {"Up/Left", new float[]{-1, -1} },
        {"Up/Right", new float[]{1, -1} },
        {"Down/Left", new float[]{-1, 1} },
        {"Down/Right", new float[]{1, 1} }
    };
}
