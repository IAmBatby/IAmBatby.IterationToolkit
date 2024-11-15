using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public static class SingletonManager
    {
        private static Dictionary<Type, MonoBehaviour> managerDict = new Dictionary<Type, MonoBehaviour>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            managerDict.Clear();
        }

        public static T GetSingleton<T>(Type type) where T : MonoBehaviour
        {
            T returnObject = null;
            if (managerDict.ContainsKey(type))
                returnObject = managerDict[type] as T;
            else
            {
                T retrivedSingleton = UnityEngine.Object.FindObjectOfType<T>();
                if (retrivedSingleton != null)
                {
                    Debug.Log("Registering New Singleton Reference For: " + retrivedSingleton.gameObject.name);
                    managerDict.Add(type, retrivedSingleton);
                    returnObject = managerDict[type] as T;
                }
            }
            return (returnObject);
        }
    }
}
