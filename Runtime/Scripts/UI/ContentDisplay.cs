using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class ContentDisplay
{
    private static ContentDisplayController _instance;
    private static ContentDisplayController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("IterationToolkit.ContentDisplayBehaviour").AddComponent<ContentDisplayController>();
                GameObject.DontDestroyOnLoad(_instance.gameObject);
            }
            return (_instance);
        }
    }

    private static void OnGUI()
    {

    }

    public static void DrawContentDisplayGroup(Vector3 position, ContentDisplayGroup group, GUIStyle style = null)
    {
        DrawContentDisplayListings(position, group.Listings, style);    
    }

    public static void DrawContentDisplayInfos(Vector3 position, List<IContentDisplayInfo> infos, GUIStyle style = null)
    {
        GetSafeStyle(ref style);
        List<Rect> rects = GetContentDisplayInfoRects(position, infos, style, 5f);
        for (int i = 0; i < rects.Count; i++)
            DrawContentDisplayInfoDirect(rects[i], infos[i], style);
    }

    public static void DrawContentDisplayListings(Vector3 position, GUIStyle style = null, params ContentDisplayListing[] listings)
    {
        DrawContentDisplayListings(position, listings.ToList(), style);
    }

    public static void DrawContentDisplayListings(Vector3 position, List<ContentDisplayListing> listings, GUIStyle style = null)
    {
        GetSafeStyle(ref style);

        List<Rect> listingRects = GetContentDisplayListingRects(position, listings.ToList(), style, 5f);

        for(int i = 0; i < listingRects.Count; i++)
        {
            List<Rect> infoRects = GetContentDisplayInfoRects(position, listings[i].Infos, style, 5f);
            for (int j = 0; j < infoRects.Count; j++)
            {
                Rect modifiedRect = new Rect(infoRects[j].x, listingRects[i].y, infoRects[j].width, listingRects[i].height);
                DrawContentDisplayInfoDirect(modifiedRect, listings[i].Infos[j], style);
            }
        }
    }

    private static void DrawContentDisplayInfoDirect(Rect rect, IContentDisplayInfo info, GUIStyle style)
    {
        GetSafeStyle(ref style);

        Texture2D oldBackground = style.normal.background;
        Color oldColor = GUI.backgroundColor;
        Color oldTextColor = GUI.contentColor;

        style.normal.background = info.ContentBackground.DisplayContent;
        GUI.contentColor = info.DisplayValue.DisplayColor;
        GUI.backgroundColor = info.ContentBackground.DisplayColor;
        GUI.Box(rect, info.DisplayValue.GetGUIContent(), style);

        GUI.backgroundColor = oldColor;
        GUI.contentColor = oldTextColor;


        if (info.ContentBorder != null)
        {
            style.normal.background = info.ContentBorder.Texture;
            GUI.Box(rect, new GUIContent(string.Empty), style);
        }

        style.normal.background = oldBackground;
    }

    public static GUIStyle GetSafeStyle(ref GUIStyle style) => style = style == null ? GUIDefaults.UI_Label : style;

    public static Rect GetContentDisplayInfoRect(Vector3 position, IContentDisplayInfo info, GUIStyle style)
    {
        return (GUIUtilities.WorldPointToSizedRect(position,info.DisplayValue.GetGUIContent(), style));
    }

    public static List<Rect> GetContentDisplayInfoRects(Vector3 position, List<IContentDisplayInfo> infos, GUIStyle style, float offset)
    {
        GetSafeStyle(ref style);
        List<Rect> returnRects = new List<Rect>();
        float xOffset = 0;

        TextAnchor old = style.alignment;
        style.alignment = TextAnchor.MiddleLeft;

        foreach (IContentDisplayInfo info in infos)
        {
            Rect displayRect = GetContentDisplayInfoRect(position, info, style);
            float offsetResults = displayRect.x + xOffset;
            displayRect = new Rect(offsetResults, displayRect.y, displayRect.width, displayRect.height);
            xOffset += displayRect.width + offset;
            returnRects.Add(displayRect);
        }

        style.alignment = old;
        return (returnRects);
    }


    public static List<Rect> GetContentDisplayListingRects(Vector3 position, List<ContentDisplayListing> listings, GUIStyle style, float offset)
    {
        GetSafeStyle(ref style);
        List<Rect> returnRects = new List<Rect>();
        float yOffset = 0;

        foreach (ContentDisplayListing listing in listings)
        {
            if (listing.Infos.Count == 0) continue;
            Rect displayRect = GetContentDisplayInfoRect(position, listing.Infos[0], style);
            if (yOffset == 0)
                yOffset -= displayRect.height;
            float offsetResult = (displayRect.y - displayRect.height) - yOffset;
            displayRect = new Rect(displayRect.x, offsetResult, displayRect.width, displayRect.height);
            yOffset += displayRect.height + offset;
            returnRects.Add(displayRect);
        }


        return (returnRects);
    }



    protected class ContentDisplayController : MonoBehaviour
    {
        private void FixedUpdate() => OnGUI();
    }
}
