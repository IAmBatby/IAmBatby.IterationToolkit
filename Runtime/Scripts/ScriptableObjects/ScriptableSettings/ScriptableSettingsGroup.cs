using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class ScriptableSettingsGroup : ScriptableObject
    {
        public ExtendedEvent OnSettingsApplied = new ExtendedEvent();

        [SerializeField] private List<ScriptableSetting> allSettings = new List<ScriptableSetting>();

        private void OnEnable()
        {
            foreach (ScriptableSetting setting in allSettings)
                setting.OnChanged.AddListener(ApplySettings);
        }

        public void ApplySettings()
        {
            Debug.Log("Something Changed! Wow!");
            OnSettingsApplied.Invoke();
        }
    }
}
