using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "ScriptableFontSetting", menuName = "IterationToolkit/Settings/ScriptableFontSetting", order = 1)]
    public class ScriptableFontSetting : ScriptableSetting
    {
        public override List<ValueSetting> GetValues()
        {
            return (new List<ValueSetting> { HeaderFont, TextFont, HeaderAutoFontSize, TextAutoFontSize });
        }

        public ObjectSetting<TMP_FontAsset> HeaderFont;
        public ObjectSetting<TMP_FontAsset> TextFont;

        public Vector2Setting HeaderAutoFontSize;
        public Vector2Setting TextAutoFontSize;
    }
}
