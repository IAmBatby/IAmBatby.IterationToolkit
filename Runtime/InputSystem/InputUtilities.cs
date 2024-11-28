#if INPUTSYSTEM_PRESENT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

namespace IterationToolkit.InputSystem
{
    public enum InputType { Keyboard, Mouse, Gamepad }
    public static class InputUtilities
    {
        public static InputDevice LastPressedDevice { get; private set; }

        public static ExtendedEvent<InputDevice> OnDeviceChanged = new ExtendedEvent<InputDevice>();

        private static Dictionary<InputType, string> inputTypeToStringDict = new Dictionary<InputType, string>()
    { {InputType.Keyboard, "<Keyboard>" }, {InputType.Mouse, "<Mouse>" }, {InputType.Gamepad, "<Gamepad>" }};


        [RuntimeInitializeOnLoadMethod]
        private static void ListenForEvents()
        {
            UnityEngine.InputSystem.InputSystem.onAnyButtonPress.Call(OnAnyButtonPress);
        }

        private static void OnAnyButtonPress(InputControl control)
        {
            LastPressedDevice = control.device;
            OnDeviceChanged.Invoke(LastPressedDevice);
        }

        public static InputAction CreateInputAction(string displayName, InputType inputType, KeyCode keyidentifier)
        {
            string inputString = keyidentifier.ToString().ToLowerInvariant();
            return (CreateInputAction(displayName, inputType, inputString));
        }

        public static InputAction CreateInputAction(string displayName, InputType inputType, string inputString)
        {
            InputAction newAction = new InputAction(displayName, binding: GetBindingString(inputType, inputString));
            newAction.Enable();
            return (newAction);
        }

        public static string GetBindingString(InputType type, string identifier)
        {
            return (inputTypeToStringDict[type] + "/" + identifier);
        }

        public static bool GetInputDown(ScriptableInputSetting setting)
        {
            if (setting == null) return (false);
            if (PreferGamepadInput())
            {
                ButtonControl button = GetGamepadButton(Gamepad.current, setting.GamepadButtonControl.Value);
                if (button != null && button.wasPressedThisFrame)
                    return (true);
                else
                    return (false);

            }
            else
            {
                if (Input.GetKeyDown(setting.InputKeyCode.Value))
                    return (true);
                else if (!string.IsNullOrEmpty(setting.VirtualAxis.Value) && Input.GetButtonDown(setting.VirtualAxis.Value))
                    return (true);
                else
                    return (false);
            }
        }

        public static float GetInputAxis(ScriptableInputSetting setting)
        {
            if (setting == null) return (0f);

            /*
            float returnValue = 0f;

            float gamePadInput = PreferGamepadInput() == false ? 0 : GetInputAxis(setting.GamepadInputAxis.Value, setting.UseRawGamepadInputValues.Value);
            float kbmInput = GetInputAxis(setting.InputAxis.Value, setting.UseRawInputValues.Value);

            if (kbmInput != 0f)
            {
                if (kbmInput > 0f && kbmInput > gamePadInput)
                    returnValue = kbmInput;
                else if (kbmInput < 0f && kbmInput < gamePadInput)
                    returnValue = kbmInput;
                else
                    returnValue = gamePadInput;
            }
            */

            if (Gamepad.current != null && LastPressedDevice == Gamepad.current)
                return (GetInputAxis(setting.GamepadInputAxis.Value, setting.UseRawGamepadInputValues.Value));
            else
                return (GetInputAxis(setting.InputAxis.Value, setting.UseRawInputValues.Value));
        }

        private static bool PreferGamepadInput()
        {
            Gamepad gamePad = Gamepad.current;
            if (gamePad != null && gamePad == CompareActivity(Keyboard.current, gamePad) && gamePad == CompareActivity(Mouse.current, gamePad))
                return (true);
            return (false);
        }

        public static InputDevice CompareActivity(InputDevice firstDevice, InputDevice secondDevice)
        {
            if (firstDevice.wasUpdatedThisFrame == true && secondDevice.wasUpdatedThisFrame == false)
                return (firstDevice);
            else if (firstDevice.wasUpdatedThisFrame == false && secondDevice.wasUpdatedThisFrame == true)
                return (secondDevice);
            else if (firstDevice.lastUpdateTime > secondDevice.lastUpdateTime)
                return (firstDevice);
            return (secondDevice);
        }

        private static ButtonControl GetGamepadButton(Gamepad gamepad, ControllerInput input)
        {
            Gamepad activeGamePad = gamepad;
            if (activeGamePad == null)
                return (null);
            return (input switch
            {
                ControllerInput.PRIMARY_SOUTH => activeGamePad.buttonSouth,
                ControllerInput.PRIMARY_EAST => activeGamePad.buttonEast,
                ControllerInput.PRIMARY_WEST => activeGamePad.buttonWest,
                ControllerInput.PRIMARY_NORTH => activeGamePad.buttonNorth,
                ControllerInput.L1 => activeGamePad.leftShoulder,
                ControllerInput.L2 => activeGamePad.leftTrigger,
                ControllerInput.L3 => activeGamePad.leftStickButton,
                ControllerInput.R1 => activeGamePad.rightShoulder,
                ControllerInput.R2 => activeGamePad.rightTrigger,
                ControllerInput.R3 => activeGamePad.rightStickButton,
                ControllerInput.DPAD_DOWN => activeGamePad.dpad.down,
                ControllerInput.DPAD_LEFT => activeGamePad.dpad.left,
                ControllerInput.DPAD_RIGHT => activeGamePad.dpad.right,
                ControllerInput.DPAD_UP => activeGamePad.dpad.up,
                ControllerInput.SELECT => activeGamePad.selectButton,
                ControllerInput.START => activeGamePad.startButton,
                _ => null
            });
        }

        private static float GetInputAxis(InputAxes axes, bool raw = false)
        {
            float value = axes switch
            {
                InputAxes.None => 0f,
                InputAxes.KeyboardHorizontal => GetKeyboardAxis(true),
                InputAxes.KeyboardVertical => GetKeyboardAxis(false),
                InputAxes.GamepadLeftHorizontal => Gamepad.current.leftStick.ReadValue().x,
                InputAxes.GamepadRightHorizontal => Gamepad.current.rightStick.ReadValue().x,
                InputAxes.GamepadLeftVertical => Gamepad.current.leftStick.ReadValue().y,
                InputAxes.GamepadRightVertical => Gamepad.current.rightStick.ReadValue().y,
                InputAxes.MouseHorizontal => ((Mouse.current.delta.ReadUnprocessedValue().x * 5) / 100),
                InputAxes.MouseVertical => ((Mouse.current.delta.ReadUnprocessedValue().y * 5) / 100),
                _ => 0f
            };

            //if (raw == true)
            //value = Mathf.RoundToInt(Mathf.Clamp(value, -1f, 1f));

            if (raw == true)
                value = Mathf.RoundToInt(value);
            return (value);
        }

        private static float GetKeyboardAxis(bool horizontal)
        {
            float returnValue = 0f;
            if (horizontal)
            {
                if (Keyboard.current.aKey.isPressed)
                    returnValue = 1f;
                else if (Keyboard.current.dKey.isPressed)
                    returnValue = -1f;
            }
            else
            {
                if (Keyboard.current.wKey.isPressed)
                    returnValue = 1f;
                else if (Keyboard.current.sKey.isPressed)
                    returnValue = -1f;
            }
            return (returnValue);
        }

    }
}

#endif
