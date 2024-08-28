using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public abstract class ScriptableSettingValue
{
    public abstract SettingsValues GetSetting();
}

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

    public override SettingsValues GetSetting()
    {
        return (new SettingsValues(KeyCode.ToString()));
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

    public override SettingsValues GetSetting()
    {
        return (new SettingsValues(Volume));
    }
}


public struct SettingsValues
{
    public enum SettingType { Int, Float, Bool, String, Enum, Object }
    public SettingType settingType;
    public int intValue;
    public float floatValue;
    public bool boolValue;
    //public enum enumValue;
    public string stringValue;
    public Object objectValue;

    public SettingsValues(Object newObjectValue)
    {
        settingType = SettingType.Object;
        objectValue = newObjectValue;

        intValue = -1;
        floatValue = -1;
        boolValue = false;
        stringValue = string.Empty;
    }

    public SettingsValues(float newFloatValue)
    {
        settingType = SettingType.Float;
        floatValue = newFloatValue;

        intValue = -1;
        objectValue = null;
        boolValue = false;
        stringValue = string.Empty;
    }

    public SettingsValues(string newStringValue)
    {
        settingType = SettingType.String;
        stringValue = newStringValue;

        intValue = -1;
        floatValue = -1;
        objectValue = null;
        boolValue = false;
    }

    public bool TryGetIntValue(out int returnIntValue)
    {
        returnIntValue = -1;
        if (settingType == SettingType.Int)
        {
            returnIntValue = intValue;
            return true;
        }
        return (false);
    }


    public bool TryGetObjectValue(out Object returnObjectValue)
    {
        returnObjectValue = null;
        if (settingType == SettingType.Object)
        {
            returnObjectValue = objectValue;
            return true;
        }
        return (false);
    }
}