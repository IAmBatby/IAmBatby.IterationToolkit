using IterationToolkit.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DisplayGuidAttribute))]
public class DisplayGuidDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // First get the attribute since it contains the range for the slider
        RangeAttribute range = attribute as RangeAttribute;

        if (!property.propertyPath.Contains("[0]")) return;
        string propertyRoot = property.propertyPath.Remove(property.propertyPath.IndexOf("."));
        SerializedProperty prop = property.serializedObject.Seek(propertyRoot);

        //SerializedProperty prop = property.

        Debug.Log(prop.name);
        string guid = string.Empty;
        for (int i = 0; i < prop.arraySize; i++)
            guid += prop.GetArrayElementAtIndex(i).uintValue;

        EditorGUI.SelectableLabel(position, guid);
    }
}
