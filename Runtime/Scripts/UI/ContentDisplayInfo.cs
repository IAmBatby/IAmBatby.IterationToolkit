using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContentDisplayInfo
{
    [field: SerializeField] public DisplayTexture ContentIcon { get; private set; }
    [field: SerializeField] public DisplayString ContentText { get; private set; }
    [field: SerializeField] public DisplayTexture ContentBackground { get; private set; }
    [field: SerializeField] public FillInfo FillInfo { get; private set; }
}
