using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GUIUtilities
{
    private static GUIStyle backgroundStyle = LabelUtilities.CreateStyle(enableRichText: true, Color.black.SetAlpha(0.45f));
    private static GUIStyle headerStyle = LabelUtilities.CreateStyle(true);
    private static Rect previousRect;
    private static float padding = 5;
    private static bool inSpace;

    public enum LayoutDisplayMode { Horizontal, Vertical};
    public static LayoutDisplayMode DisplayMode { get; set; } = LayoutDisplayMode.Horizontal;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InitializeDefaults()
    {
        backgroundStyle = LabelUtilities.CreateStyle(enableRichText: true, Color.black.SetAlpha(0.45f));
    }

    public static void RenderScriptableSettings(List<ScriptableSetting> settings, Rect rect)
    {
        for (int i = 0; i < settings.Count; i++)
            RenderScriptableSetting(settings[i], new Rect(rect.x, rect.y + (rect.height * i), rect.width, rect.height));
    }
    public static void RenderScriptableSetting(ScriptableSetting setting, Rect rect)
    {
        OpenSpace(rect);
        if (setting is ScriptableAudioSetting audioSetting)
            audioSetting.Volume.Value = RenderSliderValue(audioSetting.Volume.Value, audioSetting.DbMinMax.Value.x, audioSetting.DbMinMax.Value.y, audioSetting.name);
        CloseSpace();
    }

    public static T RenderValue<T>(T value, string header, Rect rect)
    {
        T returnValue = default;
        OpenSpace(rect);
        if (value is bool boolValue && RenderButton(boolValue, header) is T tValue)
            returnValue = tValue;
        CloseSpace();
        return (returnValue);
    }

    public static void OpenSpace(Rect rect)
    {
        if (inSpace == false)
        {
            GUILayout.BeginArea(rect, backgroundStyle);
            inSpace = true;
        }
        GUILayout.Space(padding);
        OpenDirection();
        GUILayout.Space(padding);
    }

    public static void CloseSpace()
    {
        GUILayout.Space(padding);
        CloseDirection();
        GUILayout.Space(padding);
        if (inSpace == true)
        {
            GUILayout.EndArea();
            inSpace = false;
        }
    }

    public static void OpenDirection()
    {
        if (DisplayMode == LayoutDisplayMode.Horizontal)
            GUILayout.BeginHorizontal();
        else
            GUILayout.BeginVertical();
    }

    public static void CloseDirection()
    {
        if (DisplayMode == LayoutDisplayMode.Horizontal)
            GUILayout.EndHorizontal();
        else
            GUILayout.EndVertical();
    }

    public static int RenderOptions(int index, string[] values, int xCount, Rect rect)
    {
        int returnIndex = index;
        OpenSpace(rect);
        returnIndex = GUILayout.SelectionGrid(returnIndex, values, xCount);
        CloseSpace();
        return (returnIndex);
    }

    public static void SetRenderOption<T>(T value, List<T> values, Rect rect) where T : UnityEngine.Object
    {
        int index = values.Contains(value) ? values.IndexOf(value) : 0;
        value = RenderOptions(index, values, rect);
    }

    public static void SetRenderOption<T>(ref T value, List<T> values, Rect rect)
    {
        int index = values.Contains(value) ? values.IndexOf(value) : 0;
        value = RenderOptions(index, values, rect);
    }

    public static T RenderOptions<T>(int currentIndex, List<T> values, Rect rect)
    {
        int xCount = DisplayMode == LayoutDisplayMode.Horizontal ? values.Count : 1;
        string[] valueNames = new string[values.Count];
        for (int i = 0; i < values.Count; i++)
            valueNames[i] = GetName(values[i]);
        return (values[RenderOptions(currentIndex, valueNames, xCount, rect)]);
    }

    public static T RenderOptions<T>(int currentIndex, T[] values, Rect rect)
    {
        int xCount = DisplayMode == LayoutDisplayMode.Horizontal ? values.Length : 1;
        string[] valueNames = new string[values.Length];
        for (int i = 0; i < values.Length; i++)
            valueNames[i] = GetName(values[i]);
        return (values[RenderOptions(currentIndex, valueNames, xCount, rect)]);
    }

    public static T RenderOptions<T>(int currentIndex, T[] values, string[] valueNames, Rect rect)
    {
        return (values[RenderOptions(currentIndex, valueNames, values.Length, rect)]);
    }

    public static bool RenderButton(bool value, string header)
    {
        TryRenderHeader(header);
        return (GUILayout.Toggle(value, GUIContent.none));
    }

    public static float RenderSliderValue(float value, float min, float max, string header)
    {
        TryRenderHeader(header);
        return (GUILayout.HorizontalSlider(value, min, max));
    }

    public static float RenderSliderValue(float value, Vector2 minMax, string Header)
    {
        return (RenderSliderValue(value, minMax.x, minMax.y, Header));
    }

    public static void TryRenderHeader(string header)
    {
        if (string.IsNullOrEmpty(header)) return;
        GUILayout.Label(header.ToBold().Colorize(Color.white), headerStyle);
    }

    public static string GetName<T>(T value)
    {
        if (value is UnityEngine.Object obj)
            return (obj.name);
        else
            return (value.ToString());
    }
}
