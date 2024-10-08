using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IterationToolkit
{
    
    public static class Singleton
    {
        public static T GetInstance<T>(ref T manager) where T : Manager
        {
            if (manager is T castManager)
                return (castManager);
            else
            {
                Debug.Log("Seeking Instance Of: " + typeof(T).Name);
                manager = (T)Object.FindObjectOfType(typeof(T));
            }
            return (manager as T);
        }
    }

}
