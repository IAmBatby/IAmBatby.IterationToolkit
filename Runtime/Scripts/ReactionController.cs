using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReactionController
{
    [field: SerializeField] public MaterialPreset MaterialPreset { get; private set; }
    public MaterialPlayer MaterialPlayer { get; private set; }

    public void Play(MaterialPreset preset) => MaterialPlayer?.Play(preset);
}
