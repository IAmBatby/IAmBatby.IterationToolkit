using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightingPreset", menuName = "IterationToolkit/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject
{
    [Header("Skybox")]
    public Material skyboxMaterial;
    public Color skyboxColor = Color.white;
    [Space(10)]
    [Header("Sun")]
    public Color sunColor = Color.white;
    [Space(10)]
    [Range(0f, 20000f)]
    public float sunTemperature = 10000f;
    public float sunIntensity = 1f;
    [Space(10)]
    [Header("Post-Process")]
    public Color postProcessColor = Color.white;
}
