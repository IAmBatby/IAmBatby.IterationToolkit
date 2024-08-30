using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class ScriptableSettingsGroup : ScriptableObject
    {
        public ExtendedEvent OnSettingsApplied = new ExtendedEvent();

        [SerializeField] private List<ScriptableSetting> allSettings = new List<ScriptableSetting>();

    }
}
