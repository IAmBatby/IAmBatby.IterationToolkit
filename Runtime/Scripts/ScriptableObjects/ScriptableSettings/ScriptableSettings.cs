using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class ScriptableSettings : ScriptableObject
    {
        public ExtendedEvent OnSettingsApplied = new ExtendedEvent();

        public void ApplySettings()
        {
            OnSettingApplied();
            OnSettingsApplied.Invoke();
        }

        protected virtual void OnSettingApplied()
        {

        }

        protected void UpdateSetting<T>(ref T targetValue, T newValue)
        {
            targetValue = newValue;
            ApplySettings();
        }

        private void OnValidate()
        {
            ApplySettings();
        }
    }
}
