using Codice.CM.ConfigureHelper;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class LabelUtilities
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

    public static GUIStyle CreateStyle(bool enableRichText, int fontSize = -1)
    {
        GUIStyle style = new GUIStyle();
        style.richText = enableRichText;

        if (fontSize != -1)
            style.fontSize = fontSize;
        return (style);
    }

    public static GUIStyle CreateStyle(bool enableRichText, Color backgroundColor, int fontSize = -1)
    {
        GUIStyle style = CreateStyle(enableRichText, fontSize);
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, backgroundColor);
        texture.Apply();
        style.normal.background = texture;
        return (style);
    }

    public static Color GetAlternatingColor(Color firstColor, Color secondColor, int arrayIndex)
    {
        if (arrayIndex % 2 == 0)
            return (firstColor);
        else
            return (secondColor);
    }

    public static Rect GetMousePositionRect(float xSize, float ySize)
    {
        return (new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, xSize, ySize));
    }

    public static Rect GetMousePositionRect(float xOffset, float yOffset, float xSize, float ySize)
    {
        return (new Rect(Input.mousePosition.x + xOffset, (Screen.height - Input.mousePosition.y) + yOffset, xSize, ySize));
    }
}
