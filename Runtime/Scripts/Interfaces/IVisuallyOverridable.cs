using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisuallyOverridable
{
    public MaterialCache MaterialCache { get; }

    public void OverrideVisuals(VisualPreset preset) => MaterialCache.Override(preset);
    public void OverrideVisuals(Material material) => MaterialCache.Override(material);
    public void OverrideVisuals(Material material, float time) => MaterialCache.Override(material, time);
    public void RevertVisuals() => MaterialCache.Revert();
}
