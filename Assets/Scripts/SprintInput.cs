using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintInput
{
    private string _buttonToMash;
    public string ButtonToMash { get { return _buttonToMash; } }
    private int _buttonPressTarget = 0;
    public int ButtonPressTarget { get { return _buttonPressTarget; } }
    private int _buttonPressCount = 0;
    public int ButtonPressCount { get { return _buttonPressCount; } set { _buttonPressCount = value; } }

    public SprintInput(string button, int target)
    {
        _buttonToMash = button;
        _buttonPressTarget = target;
    }
}
