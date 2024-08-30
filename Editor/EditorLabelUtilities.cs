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

        public static int HeaderFontSize = 14;
        public static int TextFontSize = 13;

        public static bool flipTable;

        public static void InsertFieldDataTable(List<string> columnHeaders, List<string> rowHeaders, List<List<SerializedProperty>> dataTable)
        {
            if (dataTable == null || dataTable.Count == 0) return;

            if (flipTable == false && GUILayout.Button("Normal View"))
                flipTable = true;
            else if (flipTable == true && GUILayout.Button("Reverse View"))
                flipTable = false;

            if (flipTable == true)
            {
                List<string> tempStrings = new List<string>(columnHeaders);
                columnHeaders = new List<string>(rowHeaders);
                rowHeaders = new List<string>(tempStrings);
                dataTable = FlipTable(dataTable);
            }

            columnHeaders.Insert(0, string.Empty);

            foreach (string rowHeader in rowHeaders)
                Debug.Log("RowHeader: " + rowHeader);


            foreach (string columnHeader in columnHeaders)
                Debug.Log("ColumnHeader: " + columnHeader);

            foreach (SerializedProperty sP in dataTable[0])
                Debug.Log(sP.displayName + " - " + sP.propertyType);


            BeginLayoutOption(LayoutOption.Vertical);

            BeginLayoutOption(LayoutOption.Horizontal);
            foreach (string columnHeader in columnHeaders)
                InsertHeader(columnHeader, LayoutOption.None, TextAnchor.MiddleCenter, HeaderColor);
            EndLayoutOption(LayoutOption.Horizontal);
            
            for (int i = 0; i < Mathf.Max(rowHeaders.Count, columnHeaders.Count); i++)
            {
                BeginLayoutOption(LayoutOption.Horizontal);
                if (rowHeaders.Count > i)
                    InsertHeader(rowHeaders[i], LayoutOption.None, TextAnchor.MiddleCenter, HeaderColor);
                else
                    InsertField(string.Empty, LayoutOption.None, GetNewStyle(fontSize: TextFontSize), Color.white);
                GUILayout.FlexibleSpace();
                foreach (List<SerializedProperty> serializedProperties in dataTable)
                {
                    if (serializedProperties.Count > i)
                        InsertField(serializedProperties[i], LayoutOption.None, GetNewStyle(fontSize: TextFontSize), GetAlternatingColor(dataTable.IndexOf(serializedProperties)));
                    else
                        InsertField(string.Empty, LayoutOption.None, GetNewStyle(fontSize: TextFontSize), Color.white);
                    GUILayout.FlexibleSpace();
                }
                EndLayoutOption(LayoutOption.Horizontal);
            }
            

            EndLayoutOption(LayoutOption.Vertical);
        }

        public static List<List<SerializedProperty>> FlipTable(List<List<SerializedProperty>> dataTable)
        {
            List<List<SerializedProperty>> returnList = new List<List<SerializedProperty>>();

            for (int i = 0; i < dataTable[0].Count; i++)
            {
                List<SerializedProperty> newList = new List<SerializedProperty>();
                foreach (List<SerializedProperty> data in dataTable)
                    newList.Add(data[i]);
                returnList.Add(newList);
            }

            return (returnList);
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
            textStyle.fontSize = HeaderFontSize;
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

        public static void BeginLayoutOption(LayoutOption layoutOption, GUIStyle style = null)
        {
            if (style != null)
            {
                if (layoutOption == LayoutOption.Horizontal)
                    EditorGUILayout.BeginHorizontal(style);
                else if (layoutOption == LayoutOption.Vertical)
                    EditorGUILayout.BeginVertical(style);
            }
            else
            {
                if (layoutOption == LayoutOption.Horizontal)
                    EditorGUILayout.BeginHorizontal();
                else if (layoutOption == LayoutOption.Vertical)
                    EditorGUILayout.BeginVertical();
            }
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
