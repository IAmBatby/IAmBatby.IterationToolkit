using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "ParticlePreset", menuName = "IterationToolkit/ParticlePreset", order = 1)]
    public class ParticlePreset : ScriptableObject
    {
        [field: SerializeField] public ParticleSystem Particle { get; private set; }

        [field: Header("Particle Values"), Space(15)]
        [field: SerializeField] public bool UseParticleValues { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public float Lifetime { get; private set; }
        [field: SerializeField] public Vector2 StartSize { get; private set; }
        [field: SerializeField] public Vector2 StartSpeed { get; private set; }
    }
}
