using IterationToolkit;
using IterationToolkit.Editor;
using UnityEditor;
using UnityEngine;
/*
[CustomPropertyDrawer(typeof(IDisplayValue<>), true)]
public class IDisplayValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorUtilities.DrawProperties(property, position, "DisplayContent", "DisplayColor");
        
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
*/
[CustomPropertyDrawer(typeof(DisplayString), true)]
public class DisplayStringDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorUtilities.DrawProperties(property, position, "String", "DisplayColor");

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(DisplayTexture), true)]
public class DisplayTextureDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorUtilities.DrawProperties(property, position, "Texture", "DisplayColor");

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
