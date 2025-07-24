using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DisplayFillMode { Opacity, Fill }

[System.Serializable]
public class FillInfo
{
    [field: SerializeField] public DisplayFillMode FillMode { get; set; }
    [field: SerializeField] public float FillValue { get; set; }
    [field: SerializeField] public Vector2 FillMinMax { get; set; }

    public float FillLerpRate => Mathf.InverseLerp(FillMinMax.x, FillMinMax.y, FillValue);

    public FillInfo(float fillValue = 1f, DisplayFillMode fillMode = DisplayFillMode.Fill)
    {
        FillMode = fillMode;
        FillMinMax = new Vector2(0, 1);
        FillValue = fillValue;
    }

    public FillInfo(Vector2 fillMinMax, float fillValue = 1f, DisplayFillMode fillMode = DisplayFillMode.Fill)
    {
        FillMode = fillMode;
        FillMinMax = fillMinMax;
        FillValue = fillValue;
    }
}
