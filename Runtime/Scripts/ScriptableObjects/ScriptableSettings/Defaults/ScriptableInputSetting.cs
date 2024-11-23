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
        public StringSetting JoystickVirtualAxis;

        public bool GetInputDown()
        {
            if (Input.GetKeyDown(InputKeyCode.Value)) return (true);
            if (!string.IsNullOrEmpty(VirtualAxis.Value) && Input.GetButtonDown(VirtualAxis.Value)) return (true);
            if (JoystickActive() && !string.IsNullOrEmpty(JoystickVirtualAxis.Value) && Input.GetButtonDown(JoystickVirtualAxis.Value)) return (true);
            return (false);
        }

        public bool JoystickActive() => (!string.IsNullOrEmpty(Input.GetJoystickNames()[0]));
    }
}
