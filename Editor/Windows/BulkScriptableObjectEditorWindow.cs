using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.ToolkitEditor
{
    public abstract class BulkScriptableObjectEditorWindow<T> : EditorWindow where T: ScriptableObject
    {
        public List<T> allSettings;
        protected T selectedScriptableSetting;


        protected void InitializeWindow()
        {
            TryPopulateData();
            Show();
        }

        protected void TryPopulateData()
        {
            if (allSettings == null || allSettings.Count == 0)
                PopulateData();
        }

        protected virtual void PopulateData()
        {
            allSettings = Resources.FindObjectsOfTypeAll<T>().ToList();
            selectedScriptableSetting = allSettings.First();
        }

        protected abstract SerializedPropertyType[] GetTypeFilters();

        protected virtual void OnGUI()
        {
            TryPopulateData();

            GUILayout.ExpandWidth(false);
            GUI.skin.label.richText = true;
            GUI.skin.textField.richText = true;
            if (allSettings != null || allSettings.Count == 0)
            {
                DrawSerializedScriptableSettingsList(allSettings);
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

    public abstract class BulkScriptableObjectEditorWindow<T, P> : BulkScriptableObjectEditorWindow<T> where T: ScriptableObject
    {
        public Dictionary<P, List<T>> settingsDict;
        private P selectedScriptableSettingsType;

        protected abstract P[] GetParentObjects(T childObject);

        protected override void PopulateData()
        {
            allSettings = Resources.FindObjectsOfTypeAll<T>().ToList();
            settingsDict = new Dictionary<P, List<T>>();
            foreach (T setting in allSettings)
            {
                foreach (P parentValue in GetParentObjects(setting))
                {
                    if (!settingsDict.ContainsKey(parentValue))
                        settingsDict.Add(parentValue, new List<T>() { setting });
                    else
                        settingsDict[parentValue].Add(setting);
                }
            }
            selectedScriptableSettingsType = settingsDict.Keys.FirstOrDefault();
            selectedScriptableSetting = settingsDict[selectedScriptableSettingsType].First();
        }

        protected override void OnGUI()
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
    }
}
