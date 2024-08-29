using IterationToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace IterationToolkit
{

    public abstract class NewScriptableSetting : ScriptableObject, ISerializationCallbackReceiver
    {
        private bool needsDefaultsApplied;
        public ExtendedEvent OnValuesChanged = new ExtendedEvent();
        public abstract List<ValueSetting> GetValues();

        public void OnValidate()
        {
            foreach (ValueSetting valueSetting in GetValues())
                valueSetting.RefreshValue();
            needsDefaultsApplied = false;
        }

        private void OnEnable()
        {
            foreach (ValueSetting valueSetting in GetValues())
                valueSetting.ScriptableSetting = this;
        }

        public void ValuesChanged()
        {
            if (Application.isPlaying)
                needsDefaultsApplied = false;
            OnValuesChanged.Invoke();

        }

        public void OnBeforeSerialize()
        {
            if (needsDefaultsApplied == false && Application.isPlaying == false)
            {
                foreach (ValueSetting valueSetting in GetValues())
                {
                    valueSetting.OnValueChanged.AddListener(ValuesChanged);
                    valueSetting.RefreshValue();
                }
                needsDefaultsApplied = true;
            }
        }

        public void OnAfterDeserialize()
        {
            needsDefaultsApplied = false;
        }
    }

    [CreateAssetMenu(fileName = "AudioSetting", menuName = "IterationToolkit/Settings/NewAudioSettings", order = 1)]
    public class NewScriptableAudioSetting : NewScriptableSetting
    {
        public override List<ValueSetting> GetValues() => new List<ValueSetting> { Volume, DbMinMax, AudioMixer };

        public ObjectSetting<AudioMixer> AudioMixer;
        public FloatSetting Volume;
        public Vector2Setting DbMinMax;
    }

    [CreateAssetMenu(fileName = "InputSetting", menuName = "IterationToolkit/Settings/NewInputSettings", order = 1)]
    public class NewScriptableInputSetting : NewScriptableSetting
    {
        public override List<ValueSetting> GetValues() => new List<ValueSetting> { InputKeyCode, VirtualAxis };

        public EnumSetting<KeyCode> InputKeyCode;
        public StringSetting VirtualAxis;
    }
}
