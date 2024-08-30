using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "ScriptableColorSetting", menuName = "IterationToolkit/Settings/ScriptableColorSetting", order = 1)]
    public class ScriptableColorSetting : ScriptableSetting
    {
        public override List<ValueSetting> GetValues() => new() { PrimaryColor, SecondaryColor, AccessibilityTexture };
        public ColorSetting PrimaryColor;
        public ColorSetting SecondaryColor;
        public ObjectSetting<Texture2D> AccessibilityTexture;
    }
}
