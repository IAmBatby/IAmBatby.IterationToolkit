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
        InvokeOnBefore(infos);
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
        InvokeOnBefore(listings.SelectMany(l => l.Infos));

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

    private static void InvokeOnBefore(params IContentDisplayInfo[] infos)
    {
        foreach (IContentDisplayInfo info in infos)
            info.OnBeforeDisplay.Invoke();
    }

    private static void InvokeOnBefore(IEnumerable<IContentDisplayInfo> infos)
    {
        foreach (IContentDisplayInfo info in infos)
            info.OnBeforeDisplay.Invoke();
    }

    private static void DrawContentDisplayInfoDirect(Rect rect, IContentDisplayInfo info, GUIStyle style)
    {
        GetSafeStyle(ref style);

        Texture2D oldBackground = style.normal.background;
        Color oldColor = GUI.backgroundColor;
        Color oldTextColor = GUI.contentColor;

        style.normal.background = info.DisplayBackground.DisplayContent;
        GUI.contentColor = info.DisplayText.DisplayColor;

        GUI.backgroundColor = new Color(0, 0, 0, 0.2f);
        GUI.Box(rect,GUIContent.none, style);

        GUI.backgroundColor = info.DisplayBackground.DisplayColor;

        Rect lerpRect = new Rect(rect.x,rect.y, Mathf.Lerp(0,rect.width,info.FillInfo.FillLerpRate),rect.height);
        //Rect lerpRect = rect;
        //GUI.DrawTexture(rect, info.DisplayBackground.Texture);
        Debug.Log("Border: " + style.border);
        RectOffset oldFlow = new RectOffset(0, 0, 0, 0);
        //style.overflow = new RectOffset(oldFlow.left, -25, oldFlow.top, oldFlow.bottom);
        //if (lerpRect.width > 0)
        //GUI.Box(lerpRect, GUIContent.none, style);
        //style.overflow = oldFlow;
        //if (Transparent == null) CreateTransparent();
        //Rect flipRect = new Rect(rect.x + lerpRect.width, rect.y, Mathf.Lerp(rect.width, 0, info.FillInfo.FillLerpRate), rect.height);

        rect = lerpRect;
        //GUI.DrawTexture(paddedRect, info.DisplayBackground.Texture, ScaleMode.ScaleAndCrop, alphaBlend: true);
        Vector2 size = style.CalcSize(info.CreateContent());
        Vector2 dif = new Vector2(lerpRect.width - size.x, lerpRect.height - size.y);
        Rect offset = new Rect(dif.x / 2, dif.y / 2, size.x / 2, size.y / 2);
        Rect calcRect = new Rect(lerpRect.x + offset.x, lerpRect.y + offset.y, size.x + dif.x, size.y + dif.y);
        GUI.DrawTexture(calcRect, info.DisplayBackground.Texture);
        GUI.DrawTexture(rect, info.DisplayBorder.Texture);
        //GUI.Box(lerpRect, GUIContent.none, style);


        style.normal.background = null;

        //GUI.Box(rect, info.CreateContent(), style);

        GUI.backgroundColor = oldColor;
        GUI.contentColor = oldTextColor;


        if (info.DisplayBorder?.Texture != null)
        {
            style.normal.background = info.DisplayBorder.Texture;
            //GUI.Box(rect, new GUIContent(string.Empty), style);
        }

        style.normal.background = oldBackground;
    }

    private static void CreateTransparent()
    {
        Texture2D refT = Texture2D.linearGrayTexture;
        Transparent = new Texture2D(refT.width, refT.height, refT.format, refT.mipmapCount, true);
        Color[] pixels = Transparent.GetPixels();
        Color empty = new Color(Color.yellow.r, Color.yellow.r, Color.yellow.b, 0.5f);
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = empty;
        Transparent.SetPixels(pixels);
        Transparent.Apply();
    }

    private static Color Clear => new Color(0, 0, 0, 0);
    private static Texture2D Transparent;

    public static GUIStyle GetSafeStyle(ref GUIStyle style) => style = style == null ? GUIDefaults.UI_Label : style;

    public static Rect GetContentDisplayInfoRect(Vector3 position, IContentDisplayInfo info, GUIStyle style)
    {
        return (GUIUtilities.WorldPointToSizedRect(position,info.CreateContent(), style));
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
        private void Awake() => CreateTransparent();
        private void FixedUpdate() => OnGUI();
    }
}
