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
    public abstract T DisplayContent { get; }
    public abstract GUIContent GetGUIContent();
}

[System.Serializable]
public class DisplayString : DisplayValue<string>
{
    [field: SerializeField] public string String { get; private set; }
    public override string DisplayContent => String;
    public override GUIContent GetGUIContent() => new GUIContent(DisplayContent.Replace("\\n", Environment.NewLine));
}


[System.Serializable]
public class DisplayTexture : DisplayValue<Texture2D>
{
    [field: SerializeField] public Texture2D Texture { get; private set; }
    public override Texture2D DisplayContent => Texture;
    public override GUIContent GetGUIContent() => new GUIContent(DisplayContent);
}
