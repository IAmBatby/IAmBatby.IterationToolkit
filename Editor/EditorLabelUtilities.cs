#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IterationToolkit.ToolkitEditor
{
    public class EditorLabelUtilities
    {
        public static bool InsertField(string fieldName, bool value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldName, EditorStyles.boldLabel);
            //EditorGUILayout.Boo
            bool returnValue = false;
            EditorGUILayout.EndHorizontal();
            return (returnValue);
        }

        public static int InsertField(string fieldName, int value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldName, EditorStyles.boldLabel);
            int returnValue = EditorGUILayout.IntField(value, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
            return (returnValue);
        }

        public static float InsertField(string fieldName, float value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldName, EditorStyles.boldLabel);
            float returnValue = EditorGUILayout.FloatField(value, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
            return (returnValue);
        }

        public static string InsertField(string fieldName, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldName, EditorStyles.boldLabel);
            string returnValue = EditorGUILayout.TextField(value, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
            return (returnValue);
        }

        public static Enum InsertField(string fieldName, Enum value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldName, EditorStyles.boldLabel);
            Enum returnValue = EditorGUILayout.EnumPopup(value, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
            return (returnValue);
        }

        public static Object InsertField(string fieldName, Object value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldName, EditorStyles.boldLabel);
            Object returnValue = EditorGUILayout.ObjectField(value, value.GetType(), false, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
            return (returnValue);
        }

        public static List<SerializedProperty> FindSerializedProperties(Object nonSerializedObject)
        {
            return (FindSerializedProperties(new SerializedObject(nonSerializedObject)));
        }

        public static List<SerializedProperty> FindSerializedProperties(SerializedObject serializedObject)
        {
            //return (FindSerializedProperties(serializedObject.GetIterator()));
            SerializedProperty mainProperty = serializedObject.GetIterator();
            List<SerializedProperty> returnList = new List<SerializedProperty>();
            if (mainProperty.NextVisible(true))
            {
                do
                    if (!returnList.Contains(mainProperty))
                        returnList.Add(mainProperty);
                while (mainProperty.NextVisible(false));
            }

            return (returnList);
        }

        public static List<SerializedProperty> FindSerializedProperties(SerializedProperty serializedProperty)
        {
            List<SerializedProperty> returnList = new List<SerializedProperty>();

            returnList.Add(serializedProperty);
            if (serializedProperty.NextVisible(true))
            {
                do
                    if (!returnList.Contains(serializedProperty))
                        returnList.Add(serializedProperty);
                while (serializedProperty.NextVisible(false));
            }

            return (returnList);
        }
    }
}

#endif
