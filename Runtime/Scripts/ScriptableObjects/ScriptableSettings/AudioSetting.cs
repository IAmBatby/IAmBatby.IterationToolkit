using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSetting", menuName = "IterationToolkit/Settings/AudioSetting", order = 1)]
public class AudioSetting : ScriptableSetting<ScriptableVolumeSettingValue>
{
    public new float Value
    {
        get => _settingValue.Volume;
        set => SetValue(new ScriptableVolumeSettingValue(value));
    }
}
