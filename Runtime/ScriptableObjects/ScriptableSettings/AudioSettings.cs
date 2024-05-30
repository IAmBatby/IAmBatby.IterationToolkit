using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "IterationToolkit/Settings/AudioSettings", order = 1)]
public class AudioSettings : ScriptableSettings
{

    [SerializeField] private MixerWithVolumeSetting _sfxSetting;
    [SerializeField] private MixerWithVolumeSetting _musicSetting;

    public float SFXVolume { get => _sfxSetting.volume; set => UpdateSetting(ref _sfxSetting.volume, value); }
    public float MusicVolume { get => _musicSetting.volume; set => UpdateSetting(ref _musicSetting.volume, value); }

    protected override void OnSettingApplied()
    {
        if (_sfxSetting.audioMixerGroup != null)
            _sfxSetting.audioMixerGroup.audioMixer.SetFloat("Volume", _sfxSetting.GetDB(SFXVolume));
        if (_musicSetting.audioMixerGroup != null)
            _musicSetting.audioMixerGroup.audioMixer.SetFloat("Volume", _musicSetting.GetDB(MusicVolume));
    }
}

[System.Serializable]
public class MixerWithVolumeSetting
{
    public AudioMixerGroup audioMixerGroup;
    [Range(0f, 1f)] public float volume;
    [SerializeField] private Vector2 dbMinMax;

    public float GetDB(float value)
    {
        return Mathf.Lerp(dbMinMax.x, dbMinMax.y, value);
    }
}
