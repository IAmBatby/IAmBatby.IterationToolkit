#if NETCODE_PRESENT

using System;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.Editor.Netcode
{
    [CustomPropertyDrawer(typeof(ExtendedGuid))]
    public class ExtendedGuidDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty array = property.Seek("m_Guid");
            TryPopulateGuid(array);

            string guid = string.Empty;
            for (int i = 0; i < array.arraySize; i++)
                guid += array.GetArrayElementAtIndex(i).uintValue;

            GUI.enabled = false;
            EditorGUI.TextField(position, label, guid);
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }

        private void TryPopulateGuid(SerializedProperty prop)
        {
            if (prop.arraySize != 0) return;

            byte[] newGuid = Guid.NewGuid().ToByteArray();
            for (int i = 0; i < newGuid.Length; i++)
            {
                prop.InsertArrayElementAtIndex(i);
                prop.GetArrayElementAtIndex(i).uintValue = newGuid[i];
            }
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
            EditorUtility.SetDirty(prop.serializedObject.targetObject);
        }
    }
}

#endif