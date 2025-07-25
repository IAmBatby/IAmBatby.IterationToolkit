using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.Editor
{
    [CustomPropertyDrawer(typeof(StyleStateColors), true)]
    public class StyleStateColorsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorUtilities.DrawProperties(property, position, "TextColor", "BackgroundColor");

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
