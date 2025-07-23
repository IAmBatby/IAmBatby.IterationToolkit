using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDisplayValue<T>
{
    public Color DisplayColor { get; }
    public T DisplayContent { get; }

    public GUIContent GetGUIContent();
}

public abstract class DisplayValue<T> : IDisplayValue<T>
{
    [field: SerializeField] public Color DisplayColor { get; private set; } = Color.white;
    public abstract T DisplayContent { get; protected set; }
    public abstract GUIContent GetGUIContent();

    public DisplayValue(T displayValue, Color displayColor)
    {
        DisplayContent = displayValue;
        DisplayColor = displayColor;
    }
}

[System.Serializable]
public class DisplayString : DisplayValue<string>
{
    public DisplayString(string displayValue, Color displayColor) : base(displayValue, displayColor) { }

    [field: SerializeField] public string String { get; private set; }
    public override string DisplayContent { get => String; protected set => String = value; }
    public override GUIContent GetGUIContent() => new GUIContent(DisplayContent.Replace("\\n", Environment.NewLine));
}


[System.Serializable]
public class DisplayTexture : DisplayValue<Texture2D>
{
    public DisplayTexture(Texture2D displayValue, Color displayColor) : base(displayValue, displayColor) { }

    [field: SerializeField] public Texture2D Texture { get; private set; }
    public override Texture2D DisplayContent { get => Texture; protected set => Texture = value; }
    public override GUIContent GetGUIContent() => new GUIContent(DisplayContent);
}
