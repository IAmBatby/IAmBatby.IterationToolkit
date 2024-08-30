#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using IterationToolkit;
using System.Linq;

namespace IterationToolkit.ToolkitEditor
{
    public enum LayoutOption { None, Horizontal, Vertical }

    public class EditorLabelUtilities
    {
        private static Color _DefaultBackgroundColor;
        public static Color DefaultBackgroundColor
        {
            get
            {
                if (_DefaultBackgroundColor.a == 0)
                {
                    var method = typeof(EditorGUIUtility)
                        .GetMethod("GetDefaultBackgroundColor", BindingFlags.NonPublic | BindingFlags.Static);
                    _DefaultBackgroundColor = (Color)method.Invoke(null, null);
                }
                return _DefaultBackgroundColor;
            }
        }

        public static Color HeaderColor = new Color(81f / 255f, 81f / 255f, 81f / 255f, 255);
        //public static Color PrimaryAlternatingColor = new Color(62f / 255f, 62f / 255f, 62f / 255f, 255);
        //public static Color SecondaryAlternatingColor = new Color(43f / 255f, 41f / 255f, 43f / 255f, 255);
        public static Color PrimaryAlternatingColor = Color.red;
        public static Color SecondaryAlternatingColor = Color.green;

        public static int HeaderFontSize = 16;
        public static int TextFontSize = 13;

        public static void InsertFieldDataTable(List<string> columnHeaders, List<string> rowHeaders, List<List<SerializedProperty>> dataTable)
        {
            if (dataTable == null || dataTable.Count == 0) return;
            columnHeaders.Insert(0, string.Empty);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            foreach (string columnHeader in columnHeaders)
                InsertHeader(columnHeader, LayoutOption.None, TextAnchor.MiddleLeft, HeaderColor);
            EditorGUILayout.EndVertical();

            for (int i = 0; i < rowHeaders.Count; i++)
            {
                EditorGUILayout.BeginVertical();
                InsertHeader(rowHeaders[i], LayoutOption.None, TextAnchor.MiddleCenter, HeaderColor);
                foreach (List<SerializedProperty> serializedProperties in dataTable)
                    InsertField(serializedProperties[i], LayoutOption.None, GetNewStyle(fontSize: TextFontSize), GetAlternatingColor(dataTable.IndexOf(serializedProperties)));
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();
        }

        public static void InsertFieldDataColumn(string headerText, List<SerializedProperty> dataList, LayoutOption layoutOption)
        {
            if (dataList == null || dataList.Count == 0) return;

            if (!string.IsNullOrEmpty(headerText))
                EditorGUILayout.LabelField(headerText.Colorize(Color.white), GetNewStyle(HeaderColor, fontSize: HeaderFontSize));

            BeginLayoutOption(layoutOption, GetNewStyle(HeaderColor));

            for (int i = 0; i < dataList.Count; i++)
                if (dataList[i] != null)
                    InsertField(dataList[i], layoutOption, GetNewStyle(fontSize: TextFontSize), GetAlternatingColor(i), GUILayout.ExpandWidth(false));

            EndLayoutOption(layoutOption);
        }

        public static void InsertFieldDataColumn<T>(string headerText, List<T> dataList, LayoutOption layoutOption, bool alternateColors = true)
        {
            if (dataList == null || dataList.Count == 0) return;

            if (!string.IsNullOrEmpty(headerText))
                EditorGUILayout.LabelField(headerText.Colorize(Color.white), GetNewStyle(HeaderColor, fontSize: HeaderFontSize));

            BeginLayoutOption(layoutOption, GetNewStyle(HeaderColor));

            for (int i = 0; i < dataList.Count; i++)
                if (dataList[i] != null)
                {
                    if (alternateColors == true)
                        InsertField(dataList[i], layoutOption, GetNewStyle(fontSize: TextFontSize), GetAlternatingColor(i), GUILayout.ExpandWidth(false));
                    else
                        InsertField(dataList[i], layoutOption, GetNewStyle(fontSize: TextFontSize), HeaderColor, GUILayout.ExpandWidth(false));
                }

            EndLayoutOption(layoutOption);
        }

        public static void InsertHeader(string headerText, LayoutOption layoutOption, TextAnchor textAnchor, Color color, params GUILayoutOption[] options)
        {
            GUIStyle backgroundStyle = GetNewStyle(color);
            backgroundStyle.alignment = textAnchor;
            BeginLayoutOption(layoutOption, backgroundStyle);

            GUIStyle textStyle = new GUIStyle(EditorStyles.boldLabel);
            textStyle.alignment = textAnchor;
            if (layoutOption == LayoutOption.None)
                textStyle.normal.background = backgroundStyle.normal.background;

            EditorGUILayout.LabelField(headerText, textStyle, options);

            EndLayoutOption(layoutOption);
        }

        public static void InsertField(SerializedProperty value, LayoutOption layoutOption, GUIStyle style, Color color, params GUILayoutOption[] options)
        {
            GUIStyle backgroundStyle = GetNewStyle(color);
            BeginLayoutOption(layoutOption, backgroundStyle);

            EditorGUILayout.PropertyField(value, GUIContent.none, options);

            EndLayoutOption(layoutOption);
        }

        public static void InsertField<T>(T value, LayoutOption layoutOption, GUIStyle style, Color color, params GUILayoutOption[] options)
        {
            GUIStyle backgroundStyle = GetNewStyle(color);
            BeginLayoutOption(layoutOption, backgroundStyle);
            EditorGUILayout.LabelField(value.ToString(), style, options);

            EndLayoutOption(layoutOption);
        }

        public static T InsertPopup<T>(List<T> popupOptions, T currentSelection, string labelText)
        {
            string[] valueNames = null;
            if (popupOptions is List<Type> typeOptions)
                valueNames = typeOptions.Select(o => o.Name).ToArray();
            else
                valueNames = popupOptions.Select(o => o.ToString()).ToArray();
            T returnValue = default;
            int returnIndex = popupOptions.IndexOf(currentSelection);

            if (!string.IsNullOrEmpty(labelText))
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(labelText);
            }

            returnValue = popupOptions[EditorGUILayout.Popup(returnIndex, valueNames)];

            if (!string.IsNullOrEmpty(labelText))
                GUILayout.EndHorizontal();

            return (returnValue);
        }

        public static List<SerializedProperty> FindSerializedProperties(Object nonSerializedObject)
        {
            return (FindSerializedProperties(new SerializedObject(nonSerializedObject)));
        }

        public static List<SerializedProperty> FindSerializedProperties(SerializedObject serializedObject)
        {
            return (FindSerializedProperties(serializedObject.GetIterator()));
        }

        public static List<SerializedProperty> FindSerializedProperties(SerializedProperty serializedProperty)
        {
            List<SerializedProperty> returnList = new List<SerializedProperty>{};
            if (serializedProperty.NextVisible(true))
            {
                do
                    if (!returnList.Contains(serializedProperty))
                        returnList.Add(serializedProperty.Copy());
                while (serializedProperty.NextVisible(false));
            }
            return (returnList);
        }

        public static void BeginLayoutOption(LayoutOption layoutOption, GUIStyle style)
        {
            if (layoutOption == LayoutOption.Horizontal)
                EditorGUILayout.BeginHorizontal(style, GUILayout.ExpandWidth(false));
            else if (layoutOption == LayoutOption.Vertical)
                EditorGUILayout.BeginVertical(style, GUILayout.ExpandWidth(false));
        }

        public static void EndLayoutOption(LayoutOption layoutOption)
        {
            if (layoutOption == LayoutOption.Horizontal)
                EditorGUILayout.EndHorizontal();
            else if (layoutOption == LayoutOption.Vertical)
                EditorGUILayout.EndVertical();
        }

        public static Color GetAlternatingColor(int arrayIndex)
        {
            if (arrayIndex % 2 == 0)
                return (PrimaryAlternatingColor);
            return (SecondaryAlternatingColor);
        }

        public static GUIStyle GetNewStyle(bool enableRichText = true, int fontSize = -1)
        {
            GUIStyle newStyle = new GUIStyle();
            newStyle.richText = enableRichText;
            newStyle.alignment = TextAnchor.MiddleLeft;

            if (fontSize != -1)
                newStyle.fontSize = fontSize;

            return newStyle;
        }

        public static GUIStyle GetNewStyle(Color backgroundColor, bool enableRichText = true, int fontSize = -1)
        {
            GUIStyle newStyle = GetNewStyle(enableRichText, fontSize);
            return newStyle.Colorize(backgroundColor);
        }
    }
}

#endif
