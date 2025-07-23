using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        /*
        Rect offset = new Rect();
        foreach (ContentDisplayInfo info in infos)
        {
            DrawContentDisplayInfo(position, info, style, offset);
            offset = new Rect(offset.x, offset.y - 50, offset.width, offset.height);
        }*/
        style = style != null ? style : GUIDefaults.UI_Label;
        List<Rect> rects = GetContentDisplayInfoRects(position, infos, style, 5f);
        for (int i = 0; i < rects.Count; i++)
            DrawContentDisplayInfoDirect(rects[i], infos[i], style);
    }
    /*
    public static void DrawContentDisplayInfo(Vector3 position, ContentDisplayInfo info, GUIStyle style = null, Rect offset = default)
    {
        style = style != null ? style : GUIDefaults.UI_Label;

        style = new GUIStyle(style);
        style.normal.background = info.ContentBackground.DisplayContent;
        Color oldColor = GUI.backgroundColor;
        Color oldTextColor = GUI.contentColor;
        GUI.contentColor = info.ContentText.DisplayColor;

        GUI.backgroundColor = info.ContentBackground.DisplayColor;

        GUIContent content = new GUIContent(info.ContentText.DisplayContent);
        Rect contentRect = GUIUtilities.WorldPointToSizedRect(position, content, style);
        Rect useRect = new Rect(offset.x + contentRect.x, offset.y + contentRect.y, offset.width + contentRect.width, offset.height + contentRect.height);
        GUI.Box(useRect, content, style); 

        GUI.backgroundColor = oldColor;
        GUI.contentColor = oldTextColor;
    }*/

    private static void DrawContentDisplayInfoDirect(Rect rect, ContentDisplayInfo info, GUIStyle style)
    {
        style = style != null ? style : GUIDefaults.UI_Label;

        Texture2D oldBackground = style.normal.background;
        Color oldColor = GUI.backgroundColor;
        Color oldTextColor = GUI.contentColor;

        style.normal.background = info.ContentBackground.DisplayContent;
        GUI.contentColor = info.ContentText.DisplayColor;
        GUI.backgroundColor = info.ContentBackground.DisplayColor;
        GUI.Box(rect, info.ContentText.GetGUIContent(), style);

        style.normal.background = oldBackground;
        GUI.backgroundColor = oldColor;
        GUI.contentColor = oldTextColor;
    }

    public static Rect GetContentDisplayInfoRect(Vector3 position, ContentDisplayInfo info, GUIStyle style)
    {
        return (GUIUtilities.WorldPointToSizedRect(position,info.ContentText.GetGUIContent(), style));
    }

    public static List<Rect> GetContentDisplayInfoRects(Vector3 position, List<ContentDisplayInfo> infos, GUIStyle style, float offset)
    {
        List<Rect> returnRects = new List<Rect>();

        float yOffset = 0;

        foreach (ContentDisplayInfo info in infos)
        {
            Rect displayRect = GetContentDisplayInfoRect(position, info, style);
            float offsetResult = (displayRect.y - displayRect.height) - yOffset;
            displayRect = new Rect(displayRect.x, offsetResult, displayRect.width, displayRect.height);
            yOffset += displayRect.height + offset;
            returnRects.Add(displayRect);
        }


        return (returnRects);
    }



    protected class ContentDisplayBehaviour : MonoBehaviour
    {
        private void FixedUpdate() => OnGUI();
    }
}
