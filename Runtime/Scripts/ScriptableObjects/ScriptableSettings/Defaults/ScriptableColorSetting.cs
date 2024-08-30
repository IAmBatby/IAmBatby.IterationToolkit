using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "ScriptableColorSetting", menuName = "IterationToolkit/Settings/ScriptableColorSetting", order = 1)]
    public class ScriptableColorSetting : ScriptableSetting
    {
        public override List<ValueSetting> GetValues()
        {
            return (new List<ValueSetting> { PrimaryColor });
        }

        public ColorSetting PrimaryColor;
    }
}
