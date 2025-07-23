using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DisplayFillMode { Opacity, Lerp }

public interface IDisplayValue<T>
{
    public Color DisplayColor { get; }
    public T DisplayContent { get; }
    public float DisplayFillRate { get; }
    public DisplayFillMode DisplayFillMode { get; }
}

public abstract class DisplayValue<T> : IDisplayValue<T>
{
    [field: SerializeField] public T DisplayContent { get; private set; }
    [field: SerializeField] public Color DisplayColor { get; private set; }
    [field: SerializeField] public float DisplayFillRate { get; private set; }
    [field: SerializeField] public DisplayFillMode DisplayFillMode { get; private set; }
}

[System.Serializable]
public class DisplayString : DisplayValue<string> { }

[System.Serializable]
public class DisplaySprite : DisplayValue<Sprite> { }

[System.Serializable]
public class DisplayTexture : DisplayValue<Texture2D> { }
