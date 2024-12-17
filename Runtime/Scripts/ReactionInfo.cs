using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [System.Serializable]
    public class ReactionInfo
    {
        [field: SerializeField] public AudioPreset AudioPreset { get; private set; }
        [field: SerializeField] public ParticlePlayer ParticlePreset { get; private set; }
    }
}
