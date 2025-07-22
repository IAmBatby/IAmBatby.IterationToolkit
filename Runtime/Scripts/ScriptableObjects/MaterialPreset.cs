using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPreset : ScriptableObject, IReactionPreset<Material>
{
    [field: SerializeField] public Material ReactionAsset { get; private set; }
}
