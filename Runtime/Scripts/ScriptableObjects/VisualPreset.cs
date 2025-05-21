using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "VisualPreset", menuName = "IterationToolkit/VisualPreset", order = 1)]
    public class VisualPreset : ScriptableObject
    {
        [field: SerializeField] public Material OverrideMaterial { get; private set; }
        [field: SerializeField] public float OverrideLength { get; private set; }
        [field: SerializeField] public bool OnlyOverrideIfReverted { get; private set; }
    }
}
