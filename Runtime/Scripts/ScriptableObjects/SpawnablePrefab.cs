using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "SpawnablePrefab", menuName = "IterationToolkit/SpawnablePrefab", order = 1)]
    public class SpawnablePrefab : ScriptableObject
    {
        public int length;
        public int width;
    }
}
