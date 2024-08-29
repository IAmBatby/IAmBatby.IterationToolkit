#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using IterationToolkit;
using IterationToolkit.ToolkitEditor;

namespace IterationToolkit
{
    /*
    [CustomEditor(typeof(NewScriptableSetting), true)]
    [CanEditMultipleObjects]
    public class ScriptableSettingEditor : Editor
    {
        List<SerializedProperty> m_Values;

        private void OnEnable()
        {
            /*
            SerializedProperty mainProperty = serializedObject.GetIterator();
            m_Values = new List<SerializedProperty>();

            if (mainProperty.NextVisible(true))
            {
                do
                {
                    // Draw movePoints property manually.
                    Debug.Log(mainProperty.displayName + " - " + mainProperty.propertyType + " - " + mainProperty.depth);
                    if (mainProperty.propertyType == SerializedPropertyType.Generic && mainProperty.boxedValue is ValueSetting)
                    {
                        m_Values.Add(mainProperty);
                        Debug.Log("Found Property");
                    }
                }
                while (mainProperty.NextVisible(false));
            
            }
        }
        public override void OnInspectorGUI ()
        {
            //base.OnInspectorGUI();

            serializedObject.Update();

            SerializedProperty mainProperty = serializedObject.GetIterator();

            if (mainProperty.NextVisible(true))
            {
                do
                {
                    // Draw movePoints property manually.
                    if (mainProperty.type == nameof(FloatSetting))
                    {
                        SerializedProperty valueProperty = mainProperty.FindPropertyRelative("_value");
                        float newFloatValue = EditorLabelUtilities.InsertField(valueProperty.displayName, valueProperty.floatValue);
                        if (newFloatValue != valueProperty.floatValue)
                            Debug.Log("yippe change");
                        valueProperty.floatValue = newFloatValue;
                    }
                    // Draw default property field.
                    else
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(mainProperty.name), true);
                    }
                }
                while (mainProperty.NextVisible(false));
                serializedObject.ApplyModifiedProperties();
            }
        }
        
    }*/
}

#endif
