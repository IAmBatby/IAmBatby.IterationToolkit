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
        public Dictionary<Type, List<ScriptableSetting>> settingsDict;
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
            settingsDict = new Dictionary<Type, List<ScriptableSetting>>();
            foreach (ScriptableSetting setting in allSettings)
            {
                Type settingType = setting.GetType();
                if (!settingsDict.ContainsKey(settingType))
                    settingsDict.Add(settingType, new List<ScriptableSetting>() { setting });
                else
                    settingsDict[settingType].Add(setting);
            }
            selectedScriptableSettingsType = settingsDict.Keys.FirstOrDefault();
            selectedScriptableSetting = settingsDict[selectedScriptableSettingsType].First();
        }

        private void OnGUI()
        {
            TryPopulateData();

            GUILayout.ExpandWidth(false);
            GUI.skin.label.richText = true;
            GUI.skin.textField.richText = true;
            if (allSettings != null || allSettings.Count == 0)
            {
                /*
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Setting Type: ", EditorStyles.boldLabel);
                GUILayout.BeginVertical();
                List<Type> settingsKeys = settingsDict.Keys.ToList();
                selectedScriptableSettingsType = settingsKeys[EditorGUILayout.Popup(settingsKeys.IndexOf(selectedScriptableSettingsType), settingsKeys.Select(t => t.Name).ToArray())];
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();*/

                selectedScriptableSettingsType = EditorLabelUtilities.InsertPopup<Type>(settingsDict.Keys.ToList(), selectedScriptableSettingsType, "Select Setting Type: ");

                GUILayout.Space(25);
                DrawSerializedScriptableSettingsList(settingsDict[selectedScriptableSettingsType]);
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

            //EditorLabelUtilities.InsertFieldDataTable(settingsWithPropertiesDict.Keys.ToList(), propertyNames, settingsWithPropertiesDict.Values.ToList());

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
