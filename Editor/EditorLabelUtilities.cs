#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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
        public static Color PrimaryAlternatingColor = new Color(62f / 255f, 62f / 255f, 62f / 255f, 255);
        public static Color SecondaryAlternatingColor = new Color(43f / 255f, 41f / 255f, 43f / 255f, 255);

        public static int HeaderFontSize = 14;
        public static int TextFontSize = 13;

        public static void FlipDataTable(ref List<List<SerializedProperty>> dataTable)
        {
            List<List<SerializedProperty>> templist = new List<List<SerializedProperty>>(dataTable);
            dataTable.Clear();

            for (int i = 0; i < templist[0].Count; i++)
            {
                List<SerializedProperty> newList = new List<SerializedProperty>();
                foreach (List<SerializedProperty> data in templist)
                    newList.Add(data[i]);
                dataTable.Add(newList);
            }
        }

        public static void FlipHeaders(ref List<string> columnHeaders, ref List<string> rowHeaders)
        {
            List<string> tempStrings = new List<string>(columnHeaders);
            columnHeaders = new List<string>(rowHeaders);
            rowHeaders = new List<string>(tempStrings);
        }

        public static void InsertFieldDataColumn(string headerText, List<SerializedProperty> dataList, LayoutOption layoutOption)
        {
            if (dataList == null || dataList.Count == 0) return;

            if (!string.IsNullOrEmpty(headerText))
                EditorGUILayout.LabelField(headerText.Colorize(Color.white), GetNewStyle(HeaderColor, fontSize: HeaderFontSize));

            BeginLayoutOption(layoutOption, GetNewStyle(HeaderColor));

            //for (int i = 0; i < dataList.Count; i++)
                //if (dataList[i] != null)
                    //InsertField(dataList[i], layoutOption, GetNewStyle(fontSize: TextFontSize), GetAlternatingColor(i), GUILayout.ExpandWidth(false));

            EndLayoutOption(layoutOption);
        }

        public static void InsertFieldDataColumn<T>(string headerText, List<T> dataList, LayoutOption layoutOption, bool alternateColors = true)
        {
            if (dataList == null || dataList.Count == 0) return;

            if (!string.IsNullOrEmpty(headerText))
                EditorGUILayout.LabelField(headerText.Colorize(Color.white), GetNewStyle(HeaderColor, fontSize: HeaderFontSize));

            BeginLayoutOption(layoutOption, GetNewStyle(HeaderColor));
            /*
            for (int i = 0; i < dataList.Count; i++)
                if (dataList[i] != null)
                {
                    if (alternateColors == true)
                        InsertField(dataList[i], layoutOption, GetNewStyle(fontSize: TextFontSize), GetAlternatingColor(i), GUILayout.ExpandWidth(false));
                    else
                        InsertField(dataList[i], layoutOption, GetNewStyle(fontSize: TextFontSize), HeaderColor, GUILayout.ExpandWidth(false));
                }

            EndLayoutOption(layoutOption);
            */
        }

        public static void InsertHeader(string headerText, LayoutOption layoutOption, TextAnchor textAnchor, GUIStyle backgroundStyle, GUIStyle textStyle, params GUILayoutOption[] options)
        {
            backgroundStyle.alignment = textAnchor;
            BeginLayoutOption(layoutOption, backgroundStyle);

            textStyle.alignment = textAnchor;
            textStyle.fontSize = HeaderFontSize;
            textStyle.normal.textColor = Color.white;
            if (layoutOption == LayoutOption.None)
                textStyle.normal.background = backgroundStyle.normal.background;

            EditorGUILayout.LabelField(headerText, textStyle, options);

            EndLayoutOption(layoutOption);
        }

        public static void InsertField(SerializedProperty value, LayoutOption layoutOption, GUIStyle backgroundStyle, params GUILayoutOption[] options)
        {
            BeginLayoutOption(layoutOption, backgroundStyle);
            Debug.Log(value.displayName + " - " + value.rectIntValue);
            EditorGUILayout.PropertyField(value, GUIContent.none, options);

            EndLayoutOption(layoutOption);
        }

        public static void InsertField<T>(T value, LayoutOption layoutOption, GUIStyle backgroundStyle, GUIStyle textStyle, params GUILayoutOption[] options)
        {
            BeginLayoutOption(layoutOption, backgroundStyle);
            EditorGUILayout.LabelField(value.ToString(), backgroundStyle, options);

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

            returnValue = InsertPopup<T>(popupOptions, valueNames, currentSelection, labelText);

            return (returnValue);
        }

        public static T InsertPopup<T>(List<T> popupOptions, string[] popupNames, T currentSelection, string labelText)
        {
            int returnIndex = popupOptions.IndexOf(currentSelection);

            if (!string.IsNullOrEmpty(labelText))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(labelText);
            }

            T returnValue = popupOptions[EditorGUILayout.Popup(returnIndex, popupNames)];

            if (!string.IsNullOrEmpty(labelText))
                EditorGUILayout.EndHorizontal();

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

        public static Rect BeginLayoutOption(LayoutOption layoutOption, GUIStyle style = null, params GUILayoutOption[] options)
        {
            if (style != null)
            {
                if (layoutOption == LayoutOption.Horizontal)
                    return (EditorGUILayout.BeginHorizontal(style, options));
                else if (layoutOption == LayoutOption.Vertical)
                    return (EditorGUILayout.BeginVertical(style, options));
            }
            else
            {
                if (layoutOption == LayoutOption.Horizontal)
                    return (EditorGUILayout.BeginHorizontal(options));
                else if (layoutOption == LayoutOption.Vertical)
                    return (EditorGUILayout.BeginVertical(options));
            }
            return (new Rect());
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

        public static GUIStyle GetAlternatingStyle(GUIStyle firstStyle, GUIStyle secondStyle, int collectionIndex)
        {
            if (collectionIndex % 2 == 0)
                return (firstStyle);
            return (secondStyle);
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
