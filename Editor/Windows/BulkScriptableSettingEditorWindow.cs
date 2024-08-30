using IterationToolkit;
using IterationToolkit.ToolkitEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.ToolkitEditor
{
    public class BulkScriptableSettingEditorWindow : BulkScriptableObjectEditorWindow<ScriptableSetting, Type>
    {
        [MenuItem("Tools/TestBulkEditor")]
        public static void OpenWindow()
        {
            BulkScriptableSettingEditorWindow window = GetWindow<BulkScriptableSettingEditorWindow>();
            window.TryPopulateData();
            window.Show();
        }
        protected override Type[] GetParentObjects(ScriptableSetting childObject)
        {
            if (childObject != null)
                return (new[] { childObject.GetType() });
            return (null);
        }

        protected override SerializedPropertyType[] GetTypeWhitelist()
        {
            return (new SerializedPropertyType[] { SerializedPropertyType.Generic });
        }
    }
}
