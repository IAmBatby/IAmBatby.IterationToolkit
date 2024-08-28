using IterationToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public abstract class NewScriptableSetting : ScriptableObject
{
    public abstract List<ValueSetting> GetValues();

    public abstract void SetValues(List<ValueSetting> values);
}

[CreateAssetMenu(fileName = "AudioSetting", menuName = "IterationToolkit/Settings/NewAudioSettings", order = 1)]
public class NewScriptableAudioSetting : NewScriptableSetting
{
    [field: SerializeReference] public ObjectSetting<AudioMixer> audioMixer;
    [SerializeField] public FloatSetting volume;
    [SerializeField] public Vector2Setting dbMinMax;

    public override List<ValueSetting> GetValues()
    {
        return new List<ValueSetting> { volume, dbMinMax, audioMixer };
    }

    public override void SetValues(List<ValueSetting> values)
    {
        throw new NotImplementedException();
    }
}
