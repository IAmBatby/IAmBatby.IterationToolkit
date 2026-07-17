#if INPUTSYSTEM_PRESENT

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace IterationToolkit.InputSystem
{
    public static class ExtendedInputManager
    {
        private static Dictionary<ScriptableInputSetting, ExtendedInputAction> actionDict = new Dictionary<ScriptableInputSetting, ExtendedInputAction>();

        [RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset() => actionDict.Clear();

        public static void RegisterKeyInputAction(ScriptableInputSetting input, Action action, bool clearPrevious = true)
        {
            if (actionDict.TryGetValue(input, out ExtendedInputAction existingAction) && clearPrevious)
                existingAction.OnContextChanged.ClearListeners();
            else
                actionDict[input] = new ExtendedInputAction(InputUtilities.CreateInputAction("Control", InputType.Keyboard, input.InputKeyCode.Value));

            actionDict[input].OnContextChanged.AddListener((CallbackContext ctx) => { if (ctx.started) action(); });
        }


        public static void AddKeyDownListener(this ScriptableInputSetting input, Action action, bool clearPrevious = true)
        {
            RegisterKeyInputAction(input, action, clearPrevious);
        }
    }
}


#endif