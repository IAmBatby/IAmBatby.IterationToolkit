using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "IterationToolkit/Levels/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string SceneName => GetSceneName();
    public string defaultSceneName;
    public string levelName;

    protected virtual string GetSceneName()
    {
        return (defaultSceneName);
    }
}
