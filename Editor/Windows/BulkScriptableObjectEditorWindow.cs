using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.ToolkitEditor
{
    public abstract class BulkScriptableObjectEditorWindow<T,P> : EditorWindow where T : ScriptableObject
    {

        public List<T> allSettings;
        public Dictionary<P, List<T>> settingsDict;
        private P selectedScriptableSettingsType;
        private T selectedScriptableSetting;


        protected static void InitializeWindow()
        {

        }

        protected void TryPopulateData()
        {
            if (allSettings == null || allSettings.Count == 0 || settingsDict == null)
                PopulateData();
        }

        private void PopulateData()
        {
            allSettings = Resources.FindObjectsOfTypeAll<T>().ToList();
            settingsDict = new Dictionary<P, List<T>>();
            foreach (T setting in allSettings)
            {
                P settingType = GetParentObject(setting);
                if (!settingsDict.ContainsKey(settingType))
                    settingsDict.Add(settingType, new List<T>() { setting });
                else
                    settingsDict[settingType].Add(setting);
            }
            selectedScriptableSettingsType = settingsDict.Keys.FirstOrDefault();
            selectedScriptableSetting = settingsDict[selectedScriptableSettingsType].First();
        }

        protected abstract P GetParentObject(T childObject);
        protected abstract SerializedPropertyType[] GetTypeFilters();

        private void OnGUI()
        {
            TryPopulateData();

            GUILayout.ExpandWidth(false);
            GUI.skin.label.richText = true;
            GUI.skin.textField.richText = true;
            if (allSettings != null || allSettings.Count == 0)
            {
                selectedScriptableSettingsType = EditorLabelUtilities.InsertPopup<P>(settingsDict.Keys.ToList(), selectedScriptableSettingsType, "Select Setting Type: ");

                GUILayout.Space(25);
                DrawSerializedScriptableSettingsList(settingsDict[selectedScriptableSettingsType]);
            }
        }
            
        public void DrawSerializedScriptableSettingsList(List<T> settings)
        {
            Dictionary<string, List<SerializedProperty>> settingsWithPropertiesDict = new Dictionary<string, List<SerializedProperty>>();
            List<string> propertyNames = new List<string>();
            List<SerializedObject> serializedSettings = new List<SerializedObject>();
            foreach (T setting in settings)
            {
                (SerializedObject, List<SerializedProperty>) results = GetScriptableObjectSerializedValues(setting, GetTypeFilters());
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

        protected virtual (SerializedObject, List<SerializedProperty>) GetScriptableObjectSerializedValues(T scriptableObject, params SerializedPropertyType[] filterTypes)
        {
            SerializedObject serializedSetting = new SerializedObject(scriptableObject);
            List<SerializedProperty> serializedProperties = new List<SerializedProperty>();
            foreach (SerializedProperty serializedProperty in EditorLabelUtilities.FindSerializedProperties(serializedSetting))
            {
                if (filterTypes == null || filterTypes.Length == 0)
                    serializedProperties.Add(serializedProperty);
                else if (filterTypes.Contains(serializedProperty.propertyType))
                    serializedProperties.Add(serializedProperty);
            }
            return (serializedSetting, serializedProperties);
        }

    }
}
