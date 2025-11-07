using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.Editor
{
    public class DataFieldTable
    {
        public bool flipTable;
        public int minWidth = 125;

        public List<string> columnHeaders;
        public List<string> rowHeaders;
        public List<string> adjustedColumnHeaders;
        public List<List<SerializedProperty>> dataTable;

        private GUIStyle headerBackgroundStyle;
        private GUIStyle primaryAlternatingBackgroundStyle;
        private GUIStyle secondaryAlternatingBackgroundStyle;

        private GUIStyle fieldTextStyle;
        private GUIStyle headerTextStyle;
        private Color HeaderColor => EditorLabelUtilities.HeaderColor;

        private Vector2 currentScrollValue;

        public delegate void OnDrawPropertyEvent(SerializedProperty property);

        public DataFieldTable(List<string> newColumnHeaders, List<string> newRowHeaders, List<List<SerializedProperty>> newDataTable)
        {
            columnHeaders = newColumnHeaders;
            rowHeaders = newRowHeaders;
            dataTable = newDataTable;

            adjustedColumnHeaders = new List<string>(columnHeaders);
            adjustedColumnHeaders.Insert(0, string.Empty);

            fieldTextStyle = EditorLabelUtilities.GetNewStyle(fontSize: EditorLabelUtilities.TextFontSize);

            headerBackgroundStyle = EditorLabelUtilities.GetNewStyle(HeaderColor);
            primaryAlternatingBackgroundStyle = EditorLabelUtilities.GetNewStyle(EditorLabelUtilities.PrimaryAlternatingColor);
            secondaryAlternatingBackgroundStyle = EditorLabelUtilities.GetNewStyle(EditorLabelUtilities.SecondaryAlternatingColor);

            headerTextStyle = new GUIStyle(EditorStyles.boldLabel);
        }

        public void DrawTable()
        {
            try
            {
                if (dataTable == null || dataTable.Count == 0) return;

                bool cachedFlipTable = flipTable;

                minWidth = EditorGUILayout.IntField(minWidth);

                if (flipTable == false && GUILayout.Button("Normal View"))
                    flipTable = true;
                else if (flipTable == true && GUILayout.Button("Reverse View"))
                    flipTable = false;

                if (flipTable != cachedFlipTable)
                    FlipTable();

                GUILayout.Space(25);

                EditorLabelUtilities.BeginLayoutOption(LayoutOption.Vertical);

                ///// #1 Start
                
                GUIStyle scrollStyle = new GUIStyle();
                scrollStyle.contentOffset = new Vector2(minWidth, 0);
                currentScrollValue = EditorGUILayout.BeginScrollView(currentScrollValue, scrollStyle);

                EditorLabelUtilities.BeginLayoutOption(LayoutOption.Horizontal, null);

                for (int i = 0; i < adjustedColumnHeaders.Count; i++)
                {
                    if (i == 0)
                        EditorLabelUtilities.InsertHeader(adjustedColumnHeaders[i], LayoutOption.None, TextAnchor.MiddleCenter, headerBackgroundStyle, headerTextStyle, GUILayout.MaxWidth(minWidth), GUILayout.MinWidth(minWidth));
                    else
                        EditorLabelUtilities.InsertHeader(adjustedColumnHeaders[i], LayoutOption.None, TextAnchor.MiddleCenter, headerBackgroundStyle, headerTextStyle, GUILayout.MinWidth(minWidth));
                }
                EditorLabelUtilities.EndLayoutOption(LayoutOption.Horizontal);

                ///// #1 End

                ///// #2 Start



                ///// #2 End

                ///// #3 Start


                for (int i = 0; i < Mathf.Max(rowHeaders.Count, adjustedColumnHeaders.Count); i++)
                {
                    GUIStyle style = EditorLabelUtilities.GetAlternatingStyle(primaryAlternatingBackgroundStyle, secondaryAlternatingBackgroundStyle, i);
                    EditorLabelUtilities.BeginLayoutOption(LayoutOption.Horizontal, style);

                    if (rowHeaders.Count > i)
                        EditorLabelUtilities.InsertHeader(rowHeaders[i], LayoutOption.None, TextAnchor.MiddleLeft, style, fieldTextStyle, GUILayout.MaxWidth(minWidth), GUILayout.MinWidth(minWidth));
                    Debug.Log(rowHeaders[i] + ": " + dataTable.Count);
                    foreach (List<SerializedProperty> serializedProperties in dataTable)
                        if (serializedProperties.Count > i)
                            EditorLabelUtilities.InsertField(serializedProperties[i], LayoutOption.None, null, GUILayout.MinWidth(minWidth));

                    EditorLabelUtilities.EndLayoutOption(LayoutOption.Horizontal);
                }

                ///// #3 End
                
                EditorLabelUtilities.EndLayoutOption(LayoutOption.Vertical);

                EditorGUILayout.EndScrollView();
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("control 2's"))
                    Debug.LogException(e);
            }
        }

        public void FlipTable()
        {
            Debug.Log("Flipping Table");
            EditorLabelUtilities.FlipHeaders(ref columnHeaders, ref rowHeaders);
            EditorLabelUtilities.FlipDataTable(ref dataTable);
            adjustedColumnHeaders = new List<string>(columnHeaders);
            adjustedColumnHeaders.Insert(0, string.Empty);
        }
    }
}