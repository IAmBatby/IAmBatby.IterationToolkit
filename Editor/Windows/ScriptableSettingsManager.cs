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
        private Type selectedScriptableSettingsType;
        private ScriptableSetting selectedScriptableSetting;

        [MenuItem("IterationToolkit/ScriptableSettings Manager")]
        public static void OpenWindow()
        {
            ScriptableSettingsManager window = GetWindow<ScriptableSettingsManager>();
            window.TryPopulateData();
            window.Show();
        }

        private void TryPopulateData()
        {
            if (allSettings == null || allSettings.Count == 0 || settingsDict == null)
                PopulateData();
        }

        private void PopulateData()
        {
            allSettings = Resources.FindObjectsOfTypeAll<ScriptableSetting>().ToList();
            settingsDict = new Dictionary<Type, ScriptableSetting>();
            foreach (ScriptableSetting setting in allSettings)
                if (!settingsDict.ContainsKey(setting.GetType()))
                    settingsDict.Add(setting.GetType(), setting);
            selectedScriptableSettingsType = settingsDict.Keys.FirstOrDefault();
            selectedScriptableSetting = settingsDict[selectedScriptableSettingsType];
        }

        private void OnGUI()
        {
            TryPopulateData();

            //GUILayout.ExpandWidth(false);
            GUI.skin.label.richText = true;
            GUI.skin.textField.richText = true;
            if (allSettings != null || allSettings.Count == 0)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Setting Type: ", EditorStyles.boldLabel);
                GUILayout.BeginVertical();
                List<Type> settingsKeys = settingsDict.Keys.ToList();
                if (selectedScriptableSettingsType != null && settingsKeys.Contains(selectedScriptableSettingsType))
                    selectedScriptableSettingsType = settingsKeys[EditorGUILayout.Popup(settingsKeys.IndexOf(selectedScriptableSettingsType), settingsKeys.Select(t => t.Name).ToArray())];
                else
                    selectedScriptableSettingsType = settingsKeys.First();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Setting: ", EditorStyles.boldLabel);
                GUILayout.BeginVertical();
                List<ScriptableSetting> settingList = new List<ScriptableSetting>();
                foreach (ScriptableSetting scriptableSetting in allSettings)
                    if (scriptableSetting.GetType() == selectedScriptableSettingsType)
                        settingList.Add(scriptableSetting);
                if (selectedScriptableSetting != null && settingList.Contains(selectedScriptableSetting))
                    selectedScriptableSetting = settingList[EditorGUILayout.Popup(settingList.IndexOf(selectedScriptableSetting), settingList.Select(s => s.name).ToArray())];
                else
                    selectedScriptableSetting = settingList.First();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                GUILayout.Space(15);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(selectedScriptableSetting.name, EditorStyles.boldLabel);
                if (Selection.activeObject != selectedScriptableSetting)
                    if (GUILayout.Button("Click To Inspect"))
                        Selection.activeObject = selectedScriptableSetting;
                GUILayout.EndHorizontal();

                //DrawSerializedScriptableSetting(settingList[selectedSettingIndex], LayoutOption.Vertical);

                GUILayout.Space(15);

                DrawSerializedScriptableSettingsList(settingList);
            }
        }

        public void DrawSerializedScriptableSettingsList(List<ScriptableSetting> settings)
        {
            Dictionary<string, List<SerializedProperty>> settingsWithPropertiesDict = new Dictionary<string, List<SerializedProperty>>();
            List<string> propertyNames = new List<string>();
            List<SerializedObject> serializedSettings = new List<SerializedObject>();
            foreach (ScriptableSetting setting in settings)
            {
                (SerializedObject, List<SerializedProperty>) results = GetScriptableSettingValues(setting);
                serializedSettings.Add(results.Item1);
                settingsWithPropertiesDict.Add(setting.name, results.Item2);
                if (propertyNames.Count == 0)
                    foreach (SerializedProperty serializedProperty in results.Item2)
                        propertyNames.Add(serializedProperty.displayName);
            }

            EditorLabelUtilities.InsertFieldDataTable(settingsWithPropertiesDict.Keys.ToList(), propertyNames, settingsWithPropertiesDict.Values.ToList());

            foreach (SerializedObject serializedObject in serializedSettings)
                serializedObject.ApplyModifiedProperties();
        }

        public (SerializedObject, List<SerializedProperty>) GetScriptableSettingValues(ScriptableSetting setting)
        {
            SerializedObject serializedSetting = new SerializedObject(setting);
            List<SerializedProperty> serializedProperties = new List<SerializedProperty>();
            foreach (SerializedProperty serializedProperty in EditorLabelUtilities.FindSerializedProperties(serializedSetting))
                if (serializedProperty.propertyType == SerializedPropertyType.Generic)
                    serializedProperties.Add(serializedProperty);
            return (serializedSetting, serializedProperties);
        }

    }
}

#endif
