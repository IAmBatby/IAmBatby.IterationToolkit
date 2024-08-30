using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "ScriptableInputSetting", menuName = "IterationToolkit/Settings/ScriptableInputSetting", order = 1)]
    public class ScriptableInputSetting : ScriptableSetting
    {
        public override List<ValueSetting> GetValues()
        {
            return (new List<ValueSetting> { InputKeyCode, VirtualAxis });
        }

        public EnumSetting<KeyCode> InputKeyCode;
        public StringSetting VirtualAxis;

        public bool GetInputDown()
        {
            return (Input.GetKeyDown(InputKeyCode.Value) || (!string.IsNullOrEmpty(VirtualAxis.Value) && Input.GetButtonDown(VirtualAxis.Value)));
        }
    }
}
