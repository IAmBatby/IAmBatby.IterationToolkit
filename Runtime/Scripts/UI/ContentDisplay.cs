using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContentDisplay
{
    private static ContentDisplayBehaviour _instance;
    private static ContentDisplayBehaviour Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("IterationToolkit.ContentDisplayBehaviour").AddComponent<ContentDisplayBehaviour>();
                GameObject.DontDestroyOnLoad(_instance.gameObject);
            }
            return (_instance);
        }
    }

    private static void OnGUI()
    {

    }

    public static void DrawContentDisplayInfos(Vector3 position, List<ContentDisplayInfo> infos, GUIStyle style = null)
    {
        Rect offset = new Rect();
        foreach (ContentDisplayInfo info in infos)
        {
            DrawContentDisplayInfo(position, info, style, offset);
            offset = new Rect(offset.x, offset.y - 50, offset.width, offset.height);
        }
    }

    public static void DrawContentDisplayInfo(Vector3 position, ContentDisplayInfo info, GUIStyle style = null, Rect offset = default)
    {
        if (style == null)
            style = GUIDefaults.UI_Label;

        style = new GUIStyle(style);
        style.normal.background = info.ContentBackground.DisplayContent;
        Color oldColor = GUI.backgroundColor;
        Color oldTextColor = GUI.contentColor;
        GUI.contentColor = info.ContentValue.DisplayColor;

        GUI.backgroundColor = info.ContentBackground.DisplayColor;

        GUIContent content = new GUIContent(info.ContentValue.DisplayContent);
        Rect contentRect = GUIUtilities.WorldPointToSizedRect(position, content, style);
        Rect useRect = new Rect(offset.x + contentRect.x, offset.y + contentRect.y, offset.width + contentRect.width, offset.height + contentRect.height);
        GUI.Box(useRect, content, style); 

        GUI.backgroundColor = oldColor;
        GUI.contentColor = oldTextColor;
    }



    protected class ContentDisplayBehaviour : MonoBehaviour
    {
        private void FixedUpdate() => OnGUI();
    }
}
