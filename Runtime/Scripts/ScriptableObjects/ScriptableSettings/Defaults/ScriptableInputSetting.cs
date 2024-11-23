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

        public enum ControllerInput
        {
            PRIMARY_01, PRIMARY_02, PRIMARY_03, PRIMARY_04,
            DPAD_LEFT, DPAD_RIGHT, DPAD_UP, DPAD_DOWN,
            R1, R2, R3, L1, L2, L3, SELECT, START, HOME, TOUCH_PAD
        }

        public EnumSetting<KeyCode> InputKeyCode;
        public StringSetting VirtualAxis;
        public EnumSetting<ControllerInput> JoystickVirtualAxis;

        public bool GetInputDown()
        {
            bool returnValue = false;
            if (JoystickActive() && GetControllerInputDown(JoystickVirtualAxis.Value))
                returnValue = true;
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

        private bool GetControllerInputDown(ControllerInput controllerInput)
        {
            return (controllerInput switch
            {
                ControllerInput.PRIMARY_01 => GetDown(KeyCode.Joystick1Button1),
                ControllerInput.PRIMARY_02 => GetDown(KeyCode.Joystick1Button0),
                ControllerInput.PRIMARY_03 => GetDown(KeyCode.Joystick1Button2),
                ControllerInput.PRIMARY_04 => GetDown(KeyCode.Joystick1Button3),
                ControllerInput.L1 => GetDown(KeyCode.Joystick1Button4),
                ControllerInput.R1 => GetDown(KeyCode.Joystick1Button5),
                ControllerInput.SELECT => GetDown(KeyCode.Joystick1Button6),
                ControllerInput.START => GetDown(KeyCode.Joystick1Button7),
                ControllerInput.L3 => GetDown(KeyCode.Joystick1Button8),
                ControllerInput.R3 => GetDown(KeyCode.Joystick1Button9),
                ControllerInput.HOME => GetDown(KeyCode.Joystick1Button12),
                ControllerInput.TOUCH_PAD => GetDown(KeyCode.Joystick1Button13),
                _ => false
            });
        }

        private bool GetDown(KeyCode keyCode) => (Input.GetKeyDown(keyCode));
    }
}
