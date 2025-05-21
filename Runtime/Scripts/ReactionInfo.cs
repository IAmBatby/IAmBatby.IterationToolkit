using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [System.Serializable]
    public struct ReactionInfo
    {
        [field: SerializeField] public AudioPreset AudioPreset { get; private set; }
        [field: SerializeField] public ParticlePreset ParticlePreset { get; private set; }
        [field: SerializeField] public VisualPreset VisualPreset { get; private set; }
    }
}
