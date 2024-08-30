#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IterationToolkit.ToolkitEditor
{
    public class ScriptableSettingsManager : EditorWindow
    {
        
        public List<ScriptableSetting> allSettings;
        public Dictionary<Type, ScriptableSetting> settingsDict;
        private int selectedSettingIndex;
        private int selectedTypeIndex;

        [MenuItem("IterationToolkit/ScriptableSettings Manager")]
        public static void OpenWindow()
        {
            ScriptableSettingsManager window = GetWindow<ScriptableSettingsManager>();
            window.TryPopulateData();
            window.Show();
        }

        private void TryPopulateData()
        {
            if (allSettings == null || allSettings.Count == 0 || settingsDict == null || settingsDict.Count != allSettings.Count)
                PopulateData();
        }

        private void PopulateData()
        {
            allSettings = Resources.FindObjectsOfTypeAll<ScriptableSetting>().ToList();
            settingsDict = new Dictionary<Type, ScriptableSetting>();
            foreach (ScriptableSetting setting in allSettings)
                if (!settingsDict.ContainsKey(setting.GetType()))
                    settingsDict.Add(setting.GetType(), setting);
        }

        private void OnGUI()
        {
            TryPopulateData();

            GUILayout.ExpandWidth(false);
            GUI.skin.label.richText = true;
            GUI.skin.textField.richText = true;
            if (allSettings != null || allSettings.Count == 0)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Setting Type: ", EditorStyles.boldLabel);
                GUILayout.BeginVertical();
                selectedTypeIndex = EditorGUILayout.Popup(selectedTypeIndex, settingsDict.Keys.Select(t => t.Name).ToArray());
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Setting: ", EditorStyles.boldLabel);
                GUILayout.BeginVertical();
                List<ScriptableSetting> settingList = new List<ScriptableSetting>();
                foreach (ScriptableSetting scriptableSetting in allSettings)
                    if (scriptableSetting.GetType() == settingsDict.Keys.ToArray()[selectedTypeIndex])
                        settingList.Add(scriptableSetting);
                selectedSettingIndex = EditorGUILayout.Popup(selectedSettingIndex, settingList.Select(s => s.name).ToArray());
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                GUILayout.Space(15);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(settingList[selectedSettingIndex].name, EditorStyles.boldLabel);
                if (Selection.activeObject != settingList[selectedSettingIndex])
                    if (GUILayout.Button("Click To Inspect"))
                        Selection.activeObject = settingList[selectedSettingIndex];
                GUILayout.EndHorizontal();

                DrawSerializedScriptableSetting(settingList[selectedSettingIndex]);
            }
        }

        public void DrawSerializedScriptableSetting(ScriptableSetting setting)
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(false));
            SerializedObject serializedSetting = new SerializedObject(setting);
            SerializedProperty mainProperty = serializedSetting.GetIterator();
            if (mainProperty.NextVisible(true))
            {
                do
                    if (mainProperty.propertyType == SerializedPropertyType.Generic)
                        EditorGUILayout.PropertyField(mainProperty);
                while (mainProperty.NextVisible(false));
            }
            serializedSetting.ApplyModifiedProperties();
            GUILayout.EndVertical();
        }
        
    }
}

#endif
