using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{

    public enum ControllerInput
    {
        NONE, PRIMARY_SOUTH, PRIMARY_EAST, PRIMARY_WEST, PRIMARY_NORTH,
        DPAD_LEFT, DPAD_RIGHT, DPAD_UP, DPAD_DOWN,
        R1, R2, R3, L1, L2, L3, SELECT, START
    }

    public enum InputAxes { None, KeyboardHorizontal, KeyboardVertical, MouseHorizontal, MouseVertical, GamepadLeftHorizontal, GamepadLeftVertical, GamepadRightHorizontal, GamepadRightVertical }

    [CreateAssetMenu(fileName = "ScriptableInputSetting", menuName = "IterationToolkit/Settings/ScriptableInputSetting", order = 1)]
    public class ScriptableInputSetting : ScriptableSetting
    {
        public override List<ValueSetting> GetValues()
        {
            return (new List<ValueSetting> { InputKeyCode, VirtualAxis, InputAxis, UseRawInputValues, GamepadButtonControl, GamepadInputAxis, UseRawGamepadInputValues });
        }

        [Header("Mouse & Keyboard")]
        public EnumSetting<KeyCode> InputKeyCode;
        public StringSetting VirtualAxis;
        public EnumSetting<InputAxes> InputAxis;
        public BoolSetting UseRawInputValues;

        [Space(10)]
        [Header("Gamepad")]
        public EnumSetting<ControllerInput> GamepadButtonControl;
        public EnumSetting<InputAxes> GamepadInputAxis;
        public BoolSetting UseRawGamepadInputValues;

        
        public bool GetInputDown()
        {
            bool returnValue = false;
            if (JoystickActive() && GetControllerInputDown(GamepadButtonControl.Value))
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
                ControllerInput.PRIMARY_SOUTH => GetDown(KeyCode.Joystick1Button1),
                ControllerInput.PRIMARY_EAST => GetDown(KeyCode.Joystick1Button0),
                ControllerInput.PRIMARY_WEST => GetDown(KeyCode.Joystick1Button2),
                ControllerInput.PRIMARY_NORTH => GetDown(KeyCode.Joystick1Button3),
                ControllerInput.L1 => GetDown(KeyCode.Joystick1Button4),
                ControllerInput.R1 => GetDown(KeyCode.Joystick1Button5),
                ControllerInput.SELECT => GetDown(KeyCode.Joystick1Button6),
                ControllerInput.START => GetDown(KeyCode.Joystick1Button7),
                ControllerInput.L3 => GetDown(KeyCode.Joystick1Button8),
                ControllerInput.R3 => GetDown(KeyCode.Joystick1Button9),
                _ => false
            });
        }

        private bool GetDown(KeyCode keyCode) => (Input.GetKeyDown(keyCode));
    }
}
