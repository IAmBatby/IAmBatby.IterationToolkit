using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IterationToolkit.ToolkitEditor
{
    public class DataFieldTable
    {
        public bool flipTable;
        public int minWidth = 125;

        public List<string> columnHeaders;
        public List<string> rowHeaders;
        public List<List<SerializedProperty>> dataTable;

        private GUIStyle textStyle;
        private Color HeaderColor => EditorLabelUtilities.HeaderColor;

        private Vector2 currentScrollValue;

        public DataFieldTable(List<string> newColumnHeaders, List<string> newRowHeaders, List<List<SerializedProperty>> newDataTable)
        {
            columnHeaders = newColumnHeaders;
            rowHeaders = newRowHeaders;
            dataTable = newDataTable;

            textStyle = EditorLabelUtilities.GetNewStyle(fontSize: EditorLabelUtilities.TextFontSize);
        }

        public void DrawTable()
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

            List<string> tempColumnHeaders = new List<string>(columnHeaders);
            tempColumnHeaders.Insert(0, string.Empty);

            EditorLabelUtilities.BeginLayoutOption(LayoutOption.Vertical);
            currentScrollValue = EditorGUILayout.BeginScrollView(currentScrollValue);

            EditorLabelUtilities.BeginLayoutOption(LayoutOption.Horizontal, null);
            foreach (string columnHeader in tempColumnHeaders)
            {
                if (tempColumnHeaders.IndexOf(columnHeader) == 0)
                    EditorLabelUtilities.InsertHeader(columnHeader, LayoutOption.None, TextAnchor.MiddleCenter, HeaderColor, GUILayout.MaxWidth(minWidth), GUILayout.MinWidth(minWidth));
                else
                    EditorLabelUtilities.InsertHeader(columnHeader, LayoutOption.None, TextAnchor.MiddleCenter, HeaderColor, GUILayout.MinWidth(minWidth));
            }
            EditorLabelUtilities.EndLayoutOption(LayoutOption.Horizontal);

            for (int i = 0; i < Mathf.Max(rowHeaders.Count, tempColumnHeaders.Count); i++)
            {
                EditorLabelUtilities.BeginLayoutOption(LayoutOption.Horizontal, null);
                if (rowHeaders.Count > i)
                    EditorLabelUtilities.InsertHeader(rowHeaders[i], LayoutOption.None, TextAnchor.MiddleLeft, HeaderColor, GUILayout.MaxWidth(minWidth), GUILayout.MinWidth(minWidth));
                foreach (List<SerializedProperty> serializedProperties in dataTable)
                    if (serializedProperties.Count > i)
                        EditorLabelUtilities.InsertField(serializedProperties[i], LayoutOption.None, textStyle, EditorLabelUtilities.GetAlternatingColor(dataTable.IndexOf(serializedProperties)), GUILayout.MinWidth(minWidth));
                EditorLabelUtilities.EndLayoutOption(LayoutOption.Horizontal);
            }

            EditorGUILayout.EndScrollView();
            EditorLabelUtilities.EndLayoutOption(LayoutOption.Vertical);
        }

        public void FlipTable()
        {
            Debug.Log("Flipping Table");
            EditorLabelUtilities.FlipHeaders(ref columnHeaders, ref rowHeaders);
            EditorLabelUtilities.FlipDataTable(ref dataTable);
        }
    }
}