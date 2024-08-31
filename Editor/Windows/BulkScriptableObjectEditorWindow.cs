using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.ToolkitEditor
{
    public abstract class BulkObjectEditorWindow<T> : EditorWindow where T : UnityEngine.Object
    {
        public List<T> allSettings;
        protected T selectedScriptableSetting;

        protected DataFieldTable currentDataFieldTable;

        private List<SerializedObject> serializedObjects = new List<SerializedObject>();


        protected void InitializeWindow()
        {
            TryPopulateData();
            Show();
        }

        protected virtual void TryPopulateData()
        {
            if (allSettings == null || allSettings.Count == 0)
                PopulateData();
        }

        protected virtual void PopulateData()
        {
            allSettings = Resources.FindObjectsOfTypeAll<T>().ToList();
            selectedScriptableSetting = allSettings.First();
        }

        protected virtual SerializedPropertyType[] GetTypeWhitelist() => null;
        protected virtual SerializedPropertyType[] GetTypeBlacklist() => null;

        protected virtual void OnGUI()
        {
            TryPopulateData();

            GUILayout.ExpandWidth(false);
            GUI.skin.label.richText = true;
            GUI.skin.textField.richText = true;
            if (allSettings != null || allSettings.Count == 0)
            {
                if (currentDataFieldTable != null)
                    DrawDataFieldTable();
                else
                    InitializeDataFieldTable(allSettings);
            }
        }

        public void DrawDataFieldTable()
        {
            currentDataFieldTable.DrawTable();
            foreach (SerializedObject serializedObject in serializedObjects)
                serializedObject.ApplyModifiedProperties();
        }

        public void InitializeDataFieldTable(List<T> objects)
        {
            Debug.Log("Initializing Data Field Table");
            Dictionary<string, List<SerializedProperty>> settingsWithPropertiesDict = new Dictionary<string, List<SerializedProperty>>();
            List<string> propertyNames = new List<string>();
            serializedObjects = new List<SerializedObject>();
            foreach (T genericObject in objects)
            {
                (SerializedObject, List<SerializedProperty>) results = GetScriptableObjectSerializedValues(genericObject, GetTypeWhitelist(), GetTypeBlacklist());
                serializedObjects.Add(results.Item1);
                settingsWithPropertiesDict.Add(GetDuplicateName(genericObject, settingsWithPropertiesDict.Keys.ToList()), results.Item2);
                if (propertyNames.Count == 0)
                    foreach (SerializedProperty serializedProperty in results.Item2)
                        propertyNames.Add(serializedProperty.displayName);
            }
            currentDataFieldTable = new DataFieldTable(settingsWithPropertiesDict.Keys.ToList(), propertyNames, settingsWithPropertiesDict.Values.ToList());
        }

        protected virtual string GetObjectName(T unityObject) => unityObject.name;

        protected virtual string GetDuplicateName(T unityObject, List<string> currentNames)
        {
            string defaultString = GetObjectName(unityObject);
            string returnString = defaultString;

            int counter = 1;
            while (currentNames.Contains(returnString))
            {
                returnString = defaultString + " (#" + counter + ")";
                counter++;
            }

            return (returnString);

        }

        protected virtual (SerializedObject, List<SerializedProperty>) GetScriptableObjectSerializedValues(T scriptableObject, SerializedPropertyType[] whitelists, SerializedPropertyType[] blacklists)
        {
            SerializedObject serializedSetting = new SerializedObject(scriptableObject);
            List<SerializedProperty> serializedProperties = new List<SerializedProperty>();
            foreach (SerializedProperty serializedProperty in EditorLabelUtilities.FindSerializedProperties(serializedSetting))
            {
                if (whitelists == null || whitelists.Length == 0)
                {
                    if (blacklists == null || !blacklists.Contains(serializedProperty.propertyType))
                            serializedProperties.Add(serializedProperty);
                }
                else if (whitelists.Contains(serializedProperty.propertyType))
                    if (blacklists == null || !blacklists.Contains(serializedProperty.propertyType))
                            serializedProperties.Add(serializedProperty);
            }
            return (serializedSetting, serializedProperties);
        }
    }
    public abstract class BulkScriptableObjectEditorWindow<T> : BulkObjectEditorWindow<T> where T : ScriptableObject
    {

    }

    public abstract class BulkScriptableObjectEditorWindow<T, P> : BulkScriptableObjectEditorWindow<T> where T: ScriptableObject
    {
        public Dictionary<P, List<T>> settingsDict;
        private P selectedScriptableSettingsType;

        private string[] parentNames;

        protected abstract P[] GetParentObjects(T childObject);

        protected override void TryPopulateData()
        {
            if (allSettings == null || allSettings.Count == 0 || settingsDict == null || selectedScriptableSettingsType == null)
                PopulateData();
        }

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

            List<string> newParentNames = new List<string>();
            foreach (P parent in settingsDict.Keys)
            {
                if (parent is Type typeParent)
                    newParentNames.Add(typeParent.Name);
                else
                    newParentNames.Add(parent.ToString());
            }
            parentNames = newParentNames.ToArray();
        }

        protected override void OnGUI()
        {
            TryPopulateData();

            GUILayout.ExpandWidth(false);
            GUI.skin.label.richText = true;
            GUI.skin.textField.richText = true;
            if (allSettings != null || allSettings.Count == 0)
            {
                P previousParentSelection = selectedScriptableSettingsType;
                selectedScriptableSettingsType = EditorLabelUtilities.InsertPopup<P>(settingsDict.Keys.ToList(), parentNames, selectedScriptableSettingsType, "Select Setting Type: ");

                GUILayout.Space(25);
                if (allSettings != null || allSettings.Count == 0)
                {
                    if (currentDataFieldTable != null && previousParentSelection.Equals(selectedScriptableSettingsType))
                        DrawDataFieldTable();
                    else
                        InitializeDataFieldTable(settingsDict[selectedScriptableSettingsType]);
                }
            }
        }
    }
}
