using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace IterationToolkit
{

    [CreateAssetMenu(fileName = "ScriptableAudioSetting", menuName = "IterationToolkit/Settings/ScriptableAudioSettings", order = 1)]
    public class ScriptableAudioSetting : ScriptableSetting
    {
        public override List<ValueSetting> GetValues()
        {
            return (new List<ValueSetting> { Volume, DbMinMax, AudioMixer });
        }

        public ObjectSetting<AudioMixer> AudioMixer;
        public FloatSetting Volume;
        public Vector2Setting DbMinMax;
    }
}
