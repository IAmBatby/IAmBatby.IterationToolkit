using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContentDisplayInfo
{
    public abstract IDisplayValue DisplayValue { get; }
}

public interface IContentDisplayInfo
{
    public IDisplayValue DisplayValue { get; }
    public DisplayTexture ContentBackground { get; }
    public DisplayTexture ContentBorder { get; set; }
    public FillInfo FillInfo { get; }
}

[System.Serializable]
public class ContentDisplayInfo<T> : IContentDisplayInfo where T : class, IDisplayValue
{
    [field: SerializeField] public T ContentValue { get; private set; }
    [field: SerializeField] public DisplayTexture ContentBackground { get; private set; }
    [field: SerializeField] public DisplayTexture ContentBorder { get; set; }
    [field: SerializeField] public FillInfo FillInfo { get; private set; }
    public IDisplayValue DisplayValue => ContentValue;

    public ContentDisplayInfo(T contentValue, DisplayTexture contentBackground, FillInfo fillInfo = null)
    {
        ContentValue = contentValue;
        ContentBackground = contentBackground;
        FillInfo = fillInfo != null ? fillInfo : new FillInfo();
    }
}

[System.Serializable]
public class ContentDisplayString : ContentDisplayInfo<DisplayString>
{
    public ContentDisplayString(DisplayString contentValue, DisplayTexture contentBackground, FillInfo fillInfo = null) : base(contentValue, contentBackground, fillInfo) { }
    public ContentDisplayString(string value, Color color, DisplayTexture contentBackground, FillInfo fillInfo = null) : base(new DisplayString(value, color), contentBackground, fillInfo) { }
    public ContentDisplayString(string value, DisplayTexture contentBackground, FillInfo fillInfo = null) : base(new DisplayString(value, Color.black), contentBackground, fillInfo) { }
    public ContentDisplayString(string value) : base(new DisplayString(value, Color.black), DisplayTexture.DefaultBackground, null) { }
}

[System.Serializable]
public class ContentDisplayTexture : ContentDisplayInfo<DisplayTexture>
{
    public ContentDisplayTexture(DisplayTexture contentValue, DisplayTexture contentBackground, FillInfo fillInfo = null) : base(contentValue, contentBackground, fillInfo) { }
    public ContentDisplayTexture(Texture2D value, Color color, DisplayTexture contentBackground, FillInfo fillInfo = null) : base(new DisplayTexture(value,color), contentBackground, fillInfo) { }
    public ContentDisplayTexture(Texture2D value, DisplayTexture contentBackground, FillInfo fillInfo = null) : base(new DisplayTexture(value, Color.black), contentBackground, fillInfo) { }
}
