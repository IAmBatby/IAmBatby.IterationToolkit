using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSetting", menuName = "IterationToolkit/Settings/InputSetting", order = 1)]
public class InputSetting : ScriptableSetting<ScriptableInputSettingValue>
{
    public bool GetInputDown()
    {
        if (Input.GetKeyDown(Value.KeyCode) || (!string.IsNullOrEmpty(Value.InputAxesName) && Input.GetButtonDown(Value.InputAxesName)))
            return (true);
        return (false);
    }
}
