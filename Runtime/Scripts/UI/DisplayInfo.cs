using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DisplayInfo
{
    public IContentDisplayInfo Info {  get; private set; }
    public GUIStyle DrawStyle { get; private set; }
    public Rect DrawRect { get; private set; }
    public Rect DrawFillRect { get; private set; }
    public GUIContent InfoContent { get; private set; }

}
