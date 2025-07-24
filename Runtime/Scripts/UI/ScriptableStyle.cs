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
    [field: SerializeField] public Texture2D Background { get; private set; }
    [field: SerializeField] public Texture2D Border { get; private set; }
    [field: SerializeField] public RectOffset Padding { get; private set; }
    [field: SerializeField] public bool StretchWidth { get; private set; }
    [field: SerializeField] public bool StretchHeight { get; private set; }

    [field: SerializeField] public Vector2 FixedSize { get; private set; }

    [SerializeField, HideInInspector] private GUIStyle generatedStyle;

    public GUIStyle Style => generatedStyle;

    private void OnValidate()
    {
        if (generatedStyle == null)
            generatedStyle = new GUIStyle();
        generatedStyle.richText = true;
        generatedStyle.fontSize = FontSize;
        generatedStyle.fontStyle = FontStyle;
        generatedStyle.alignment = Allignment;
        generatedStyle.font = Font;
        generatedStyle.normal.textColor = Color.white;
        generatedStyle.normal.background = Background;
        generatedStyle.padding = Padding;
        generatedStyle.stretchWidth = StretchWidth;
        generatedStyle.stretchHeight = StretchHeight;
        generatedStyle.fixedWidth = FixedSize.x;
        generatedStyle.fixedHeight = FixedSize.y;
    }
}
