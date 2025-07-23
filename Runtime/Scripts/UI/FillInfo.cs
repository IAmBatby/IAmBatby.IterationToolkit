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
}
