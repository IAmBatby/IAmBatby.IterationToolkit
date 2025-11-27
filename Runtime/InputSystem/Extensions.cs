using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit.InputSystem
{
    public static class Extensions
    {
        public static float GetAxis(this ScriptableInputSetting inputSetting)
        {
            return (InputUtilities.GetInputAxis(inputSetting));
        }
    }
}

