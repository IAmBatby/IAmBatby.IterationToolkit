using IterationToolkit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace IterationToolkit
{

    public abstract class ScriptableSetting : ScriptableObject, ISerializationCallbackReceiver
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
            try //Try catching here temporarily due to odd Order Of Execution weirdness when making new SO assets. Seemingly does work fine.
            {
                foreach (ValueSetting valueSetting in GetValues())
                    valueSetting.ScriptableSetting = this;
            }
            catch { }
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
                try //Try catching here temporarily due to odd Order Of Execution weirdness when making new SO assets. Seemingly does work fine.
                {
                    foreach (ValueSetting valueSetting in GetValues())
                    {
                        valueSetting.OnValueChanged.AddListener(ValuesChanged);
                        valueSetting.RefreshValue();
                    }
                    needsDefaultsApplied = true;
                }
                catch { }
            }
        }

        public void OnAfterDeserialize()
        {
            needsDefaultsApplied = false;
        }
    }
}

