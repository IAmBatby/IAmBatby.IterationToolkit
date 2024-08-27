using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public abstract class ScriptableSettingValue { }

[System.Serializable]
public class ScriptableInputSettingValue : ScriptableSettingValue
{
    [field: SerializeField] public KeyCode KeyCode { get; private set; }
    [field: SerializeField] public string InputAxesName { get; private set; }

    public ScriptableInputSettingValue(KeyCode newKeyCode, string newInputAxesName)
    {
        KeyCode = newKeyCode;
        InputAxesName = newInputAxesName;
    }
}

[System.Serializable]
public class ScriptableVolumeSettingValue : ScriptableSettingValue
{
    [field: SerializeField][field: Range(0f, 1f)] public float Volume { get; private set; }
    [field: SerializeField] public Vector2 DBMinMax { get; private set; }
    [field: SerializeField] public AudioMixerGroup MixerGroup { get; private set; }

    public ScriptableVolumeSettingValue(float newVolume)
    {
        Volume = newVolume;
    }
}