using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class AutoCollection : GlobalManager
    {
        public static new AutoCollection Instance => Singleton<AutoCollection>.GetInstance(ref _manager);

        private Dictionary<Type, List<MonoBehaviour>> collectionDict = new Dictionary<Type, List<MonoBehaviour>>();


        public void Add(MonoBehaviour monoBehaviour)
        {
            Type behaviourType = monoBehaviour.GetType();
            if (collectionDict.ContainsKey(behaviourType))
            {
                if (!collectionDict[behaviourType].Contains(monoBehaviour))
                    collectionDict[behaviourType].Add(monoBehaviour);
            }
            else
            {
                collectionDict.Add(behaviourType, new List<MonoBehaviour>());
            }
        }

        public void Remove(MonoBehaviour monoBehaviour)
        {

        }
    }
}
