using IterationToolkit;
using IterationToolkit.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
/*
namespace IterationToolkit
{
    [CustomPropertyDrawer(typeof(ReactionInfo))]
    public class ReactionInfoDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty audioPresetProp = Seek(property, Decorate(nameof(ReactionInfo.AudioPreset)));
            SerializedProperty particlePresetProp = Seek(property, Decorate(nameof(ReactionInfo.ParticlePreset)));
            EditorGUI.BeginProperty(position, label, property);
            Rect firstRect = new Rect(position.x, position.y, position.width / 1.5f, position.height);
            Rect secondRect = new Rect(position.x + (position.width / 1.5f) + 15, position.y, (position.width / 3) - 15, position.height);
            EditorGUI.PropertyField(firstRect, audioPresetProp, label);
            EditorGUI.PropertyField(secondRect, particlePresetProp, GUIContent.none);
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }

        private string Decorate(string name) => "<" + name + ">k__BackingField";

        private SerializedProperty Seek(SerializedProperty target, string targetField)
        {
            foreach (SerializedProperty sp in EditorLabelUtilities.FindSerializedProperties(target.Copy()))
                if (sp.name.Contains(targetField) || sp.name.Contains(Decorate(targetField)))
                    return (sp.Copy());
            return (null);
        }
    }
}
*/