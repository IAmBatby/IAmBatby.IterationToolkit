using IterationToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContentDisplayInfo
{
    public DisplayString DisplayText { get; set;  }
    public DisplayTexture DisplayIcon { get; set; }
    public DisplayTexture DisplayBackground { get; }
    public DisplayTexture DisplayBorder { get; set; }
    public FillInfo FillInfo { get; set; }

    public ExtendedEvent OnBeforeDisplay { get; }

    public GUIContent CreateContent() => new GUIContent(DisplayText.String.Replace("\\n", Environment.NewLine), DisplayIcon.Texture);
    public GUIContent CreateSizingContent();

    public void OverrideTextSizing(string sizingText);

}

[System.Serializable]
public class ContentDisplayInfo : IContentDisplayInfo
{
    [field: SerializeField] public DisplayString DisplayText { get; set; }
    [field: SerializeField] public DisplayTexture DisplayIcon { get; set; }
    [field: SerializeField] public DisplayTexture DisplayBackground { get; set; }
    [field: SerializeField] public DisplayTexture DisplayBorder { get; set; }
    [field: SerializeField] public FillInfo FillInfo { get; set; }

    public ExtendedEvent OnBeforeDisplay { get; private set; } = new ExtendedEvent();

    private DisplayString overrideSizeString;

    public GUIContent CreateSizingContent()
    {
        if (overrideSizeString != null)
            return (new GUIContent(overrideSizeString.String.Replace("\\n", Environment.NewLine), DisplayIcon.Texture));
        else
            return (new GUIContent(DisplayText.String.Replace("\\n", Environment.NewLine), DisplayIcon.Texture));
    }

    public void OverrideTextSizing(string sizingText)
    {
        if (string.IsNullOrEmpty(sizingText))
            overrideSizeString = null;
        else
            overrideSizeString = new DisplayString(sizingText);
    }


    public ContentDisplayInfo(DisplayString text = null, DisplayTexture background = null, DisplayTexture border = null, DisplayTexture icon = null, FillInfo fillInfo = null)
    {
        Construct(text, background, border, icon, fillInfo);
    }

    public ContentDisplayInfo(string newText, ScriptableStyle style)
    {
        Construct();
        DisplayText = new DisplayString(newText, style.StyleStates.Normal.TextColor);
        DisplayBackground = new DisplayTexture(style.Background, style.StyleStates.Normal.BackgroundColor);
        DisplayBorder = new DisplayTexture(style.Border, style.StyleStates.Normal.BackgroundColor);
    }

    public ContentDisplayInfo(string newText, Color newTextColor, ScriptableStyle style)
    {
        Construct();
        DisplayText = new DisplayString(newText, newTextColor);
        DisplayBackground = new DisplayTexture(style.Background, style.StyleStates.Normal.BackgroundColor);
        DisplayBorder = new DisplayTexture(style.Border, style.StyleStates.Normal.BackgroundColor);
    }

    public ContentDisplayInfo(string newText, ScriptableStyle defaultStyle, Color textColor, Color backgroundColor)
    {
        Construct();
        DisplayText = new DisplayString(newText, textColor);
        DisplayBackground = new DisplayTexture(defaultStyle.Background, backgroundColor);
        DisplayBorder = new DisplayTexture(defaultStyle.Border, defaultStyle.StyleStates.Normal.BackgroundColor);
    }

    private void Construct(DisplayString text = null, DisplayTexture background = null, DisplayTexture border = null, DisplayTexture icon = null, FillInfo fillInfo = null)
    {
        DisplayText = text != null ? text : new DisplayString(string.Empty);
        DisplayIcon = icon != null ? icon : new DisplayTexture(null);
        DisplayBackground = background != null ? background : new DisplayTexture(null);
        DisplayBorder = border != null ? border : new DisplayTexture(null);
        FillInfo = fillInfo != null ? fillInfo : new FillInfo();
    }
}
