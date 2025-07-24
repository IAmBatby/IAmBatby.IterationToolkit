using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StyleStateColors
{
    [field: SerializeField] public Color BackgroundColor { get; private set; }
    [field: SerializeField] public Color TextColor { get; private set; }
}
