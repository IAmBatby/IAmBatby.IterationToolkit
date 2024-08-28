#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IterationToolkit.ToolkitEditor
{
    public class ScriptableSettingsDebugger : EditorWindow
    {
        public List<ScriptableSetting> allSettings;
        private int selectedSettingIndex;

        [MenuItem("IterationToolkit/ScriptableSettings Manager")]
        public static void OpenWindow()
        {
            ScriptableSettingsDebugger window = GetWindow<ScriptableSettingsDebugger>();
            window.allSettings = Resources.FindObjectsOfTypeAll<ScriptableSetting>().ToList();
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.ExpandWidth(false);
            GUI.skin.label.richText = true;
            GUI.skin.textField.richText = true;

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select Setting: ", EditorStyles.boldLabel);
            selectedSettingIndex = EditorGUILayout.Popup(selectedSettingIndex, allSettings.Select(s => s.name).ToArray());
            if (GUILayout.Button("Click To Inspect"))
                Selection.activeObject = allSettings[selectedSettingIndex];
            GUILayout.EndHorizontal();

            GUILayout.Space(15);

            DrawScriptableSetting(allSettings[selectedSettingIndex]);
        }

        public void DrawScriptableSetting(ScriptableSetting setting)
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(false));
            DrawValueContainer(null, setting.GetValue());
            GUILayout.EndVertical();

        }

        public void DrawValueContainer(ScriptableSettingValue scriptableSettingValue, ValueContainer settingValue)
        {
            if (settingValue.TryGetIntValue(out int intValue))
                scriptableSettingValue.SetValue(settingValue.ValueName, EditorLabelUtilities.InsertField(settingValue.ValueName, intValue));
            else if (settingValue.TryGetFloatValue(out float floatValue))
                scriptableSettingValue.SetValue(settingValue.ValueName, EditorLabelUtilities.InsertField(settingValue.ValueName, floatValue));
            else if (settingValue.TryGetBoolValue(out bool boolValue))
                scriptableSettingValue.SetValue(settingValue.ValueName, EditorLabelUtilities.InsertField(settingValue.ValueName, boolValue));
            else if (settingValue.TryGetStringValue(out string stringValue))
                scriptableSettingValue.SetValue(settingValue.ValueName, EditorLabelUtilities.InsertField(settingValue.ValueName, stringValue));
            else if (settingValue.TryGetEnumValue(out Enum enumValue))
                scriptableSettingValue.SetValue(settingValue.ValueName, EditorLabelUtilities.InsertField(settingValue.ValueName, enumValue));
            else if (settingValue.TryGetObjectValue(out object objectValue))
            {
                if (objectValue is ScriptableSettingValue newScriptableSettingValue)
                    foreach (ValueContainer container in newScriptableSettingValue.GetValues())
                        DrawValueContainer(newScriptableSettingValue, container);
                else
                    scriptableSettingValue.SetValue(settingValue.ValueName, EditorLabelUtilities.InsertField(settingValue.ValueName, objectValue.ToString()));
            }
        }
    }
}

#endif
