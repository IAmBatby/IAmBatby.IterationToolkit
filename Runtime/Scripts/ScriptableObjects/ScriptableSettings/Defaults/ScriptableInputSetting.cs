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
            return (new List<ValueSetting> { InputKeyCode, VirtualAxis, JoystickVirtualAxis });
        }

        public EnumSetting<KeyCode> InputKeyCode;
        public StringSetting VirtualAxis;
        public StringSetting JoystickVirtualAxis;

        public bool GetInputDown()
        {
            bool returnValue = false;
            if (JoystickActive() && !string.IsNullOrEmpty(JoystickVirtualAxis.Value))
                returnValue = Input.GetButtonDown(JoystickVirtualAxis.Value);
            else if (!string.IsNullOrEmpty(VirtualAxis.Value))
                returnValue = Input.GetButtonDown(VirtualAxis.Value);
            if (Input.GetKeyDown(InputKeyCode.Value))
                returnValue = true;

            return (returnValue);
        }

        public bool JoystickActive()
        {
            string[] joysticks = Input.GetJoystickNames();
            if (joysticks == null || joysticks.Length == 0 || string.IsNullOrEmpty(joysticks[0]))
                return (false);
            return (true);
        }
    }
}
