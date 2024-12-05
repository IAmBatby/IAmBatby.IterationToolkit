using IterationToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.Editor
{
    public class BulkScriptableSettingEditorWindow : BulkScriptableObjectEditorWindow<ScriptableSetting, Type>
    {
        [MenuItem("Tools/TestBulkEditor")]
        public static void OpenWindow() => GetWindow<BulkScriptableSettingEditorWindow>().InitializeWindow();

        protected override Type[] GetParentObjects(ScriptableSetting childObject) => new[] { childObject.GetType() };

        protected override SerializedPropertyType[] GetTypeWhitelist() => new[] { SerializedPropertyType.Generic }; 
    }
}
