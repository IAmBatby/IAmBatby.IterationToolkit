using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "GlobalEvents", menuName = "IterationToolkit/GlobalEvents", order = 1)]
    public class GlobalEvents : ScriptableObject
    {
        public void LoadNewLevel(LevelData levelData)
        {
            //GlobalManager.Instance.LoadNewLevel(levelData);
        }
    }
}
