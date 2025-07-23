using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DisplayFillMode { Opacity, Fill }

[System.Serializable]
public class FillInfo
{
    [field: SerializeField] public DisplayFillMode FillMode { get; private set; }
    [field: SerializeField, Range(0f,1f)] public float FillValue { get; private set; }
    [field: SerializeField] public Vector2 FillMinMax { get; private set; }

    public FillInfo(Vector2 fillMinMax = default, float fillValue = 1f, DisplayFillMode fillMode = DisplayFillMode.Fill)
    {
        FillMode = fillMode;
        FillMinMax = fillMinMax;
        FillValue = fillValue;
    }
}
