using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace IterationToolkit.Editor
{
    /*
    [CustomPropertyDrawer(typeof(ScriptableSetting), true)]
    public class ScriptableSettingDrawer : PropertyDrawer
    {
        private bool isFoldoutOpen;
        private int foldoutOffsetX = 16;
        private int propertyOffsetY = 24;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            isFoldoutOpen = EditorGUI.BeginFoldoutHeaderGroup(position, isFoldoutOpen, GUIContent.none);

            position.x += foldoutOffsetX;
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Rect valueRect = new Rect(position.x - foldoutOffsetX, position.y, position.width, position.height);
            EditorGUI.PropertyField(valueRect, property, GUIContent.none);

            if (isFoldoutOpen)
            {
                GUILayout.BeginVertical(GUILayout.ExpandWidth(false));
                if (property.NextVisible(true))
                {
                    do
                    {
                        Debug.Log(property.name + " - " + property.propertyType);
                        if (property.propertyType == SerializedPropertyType.ObjectReference)
                            if (property.objectReferenceValue != null)
                                DrawScriptableSettingsValues(valueRect, property.objectReferenceValue as ScriptableSetting);
                    }
                    while (property.NextVisible(false));
                }
                GUILayout.EndVertical();
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();

            EditorGUI.EndFoldoutHeaderGroup();
        }

        private void DrawScriptableSettingsValues(Rect rect, ScriptableSetting scriptableSetting)
        {
            Debug.Log("rawr");
            SerializedObject serializedObject = new SerializedObject(scriptableSetting);
            SerializedProperty property = serializedObject.GetIterator();
            int counter = 1;
            if (property.NextVisible(true))
            {
                do
                {
                    Debug.Log(property.name + " - " + property.propertyType);
                    if (property.propertyType == SerializedPropertyType.Generic)
                    {
                        rect.y += (propertyOffsetY * counter);
                        EditorGUI.BeginProperty(rect, GUIContent.none, property);
                        EditorGUI.PropertyField(rect, property, GUIContent.none);
                        EditorGUI.EndProperty();
                        counter++;
                    }
                }
                while (property.NextVisible(false));
            }
        }
    }
    */
}
