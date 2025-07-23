using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableStyle", menuName = "IterationToolkit/UI/ScriptableStyle", order = 1)]
public class ScriptableStyle : ScriptableObject
{
    [field: SerializeField] public Font Font { get; private set; }
    [field: SerializeField] public int FontSize { get; private set; }
    [field: SerializeField] public FontStyle FontStyle { get; private set; }
    [field: SerializeField] public TextAnchor Allignment { get; private set; }

    [SerializeField, HideInInspector] private GUIStyle generatedStyle;

    public GUIStyle Style => generatedStyle;

    private void OnValidate()
    {
        if (generatedStyle == null)
            generatedStyle = new GUIStyle();
        generatedStyle.richText = true;
        generatedStyle.fontSize = FontSize;
        generatedStyle.fontStyle = FontStyle;
        generatedStyle.font = Font;
        generatedStyle.normal.textColor = Color.white;
    }
}
