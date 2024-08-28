#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using IterationToolkit;

namespace IterationToolkit
{
    [CustomEditor(typeof(ScriptableSetting), true)]
    [CanEditMultipleObjects]
    public class ScriptableSettingEditor : Editor
    {
    }
}

#endif
