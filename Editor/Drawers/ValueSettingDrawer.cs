using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace IterationToolkit.ToolkitEditor
{
    [CustomPropertyDrawer(typeof(ValueSetting<>), true)]
    public class ValueSettingDrawer : PropertyDrawer
    {
        protected virtual string FindValueProperty() => "_value";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect valueRect = new Rect(position.x, position.y, position.width, position.height);
            SerializedProperty valueProperty = property.FindPropertyRelative(FindValueProperty());
            EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(ObjectSetting<>), true)]
    public class ObjectSettingDrawer : ValueSettingDrawer
    {
        protected override string FindValueProperty() => "_typeValue";
    }

    [CustomPropertyDrawer(typeof(EnumSetting<>), true)]
    public class EnumSettingDrawer : ValueSettingDrawer
    {
        protected override string FindValueProperty() => "_typeValue";
    }
}



