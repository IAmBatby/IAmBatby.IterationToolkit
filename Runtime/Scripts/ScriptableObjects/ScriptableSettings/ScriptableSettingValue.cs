using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public abstract class ScriptableSettingValue
{
    public abstract List<ValueContainer> GetValues();

    public virtual void SetValue<T>(string valueName, T value) { }
}

[System.Serializable]
public class ScriptableInputSettingValue : ScriptableSettingValue
{
    [field: SerializeField] public KeyCode KeyCode { get; private set; }
    [field: SerializeField] public string InputAxesName { get; private set; }

    public ScriptableInputSettingValue(KeyCode newKeyCode, string newInputAxesName) { KeyCode = newKeyCode; InputAxesName = newInputAxesName; }

    public override List<ValueContainer> GetValues() { return new List<ValueContainer>() { new ValueContainer(KeyCode, nameof(KeyCode)), new ValueContainer(InputAxesName, nameof(InputAxesName)) }; }

    public override void SetValue<T>(string valueName, T value)
    {
        if (valueName == nameof(KeyCode) && value is Enum enumValue) KeyCode = (KeyCode)enumValue;
        if (valueName == nameof(InputAxesName) && value is string stringValue) InputAxesName = stringValue;
    }
}

[System.Serializable]
public class ScriptableVolumeSettingValue : ScriptableSettingValue
{
    [field: SerializeField][field: Range(0f, 1f)] public float Volume { get; private set; }
    [field: SerializeField] public Vector2 DBMinMax { get; private set; }
    [field: SerializeField] public AudioMixerGroup MixerGroup { get; private set; }

    public ScriptableVolumeSettingValue(float newVolume) { Volume = newVolume; }

    public override List<ValueContainer> GetValues() { return new List<ValueContainer>() { new(Volume, nameof(Volume)), new(MixerGroup, nameof(MixerGroup)), new (DBMinMax, nameof(DBMinMax))}; }

    public override void SetValue<T>(string valueName, T value)
    {
        if (valueName == nameof(Volume) && value is float floatValue) Volume = floatValue;
    }
}


