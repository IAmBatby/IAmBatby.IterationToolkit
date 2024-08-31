#if NETCODE_PRESENT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public static class Singleton
    {
        public static T GetInstance<T>(ref NetworkGlobalManager manager) where T : NetworkGlobalManager
        {
            if (manager is T castManager)
                return (castManager);
            else
            {
                Debug.Log("Seeking Instance Of: " + typeof(T).Name);
                manager = (NetworkGlobalManager)Object.FindObjectOfType(typeof(T));
            }
            return (manager as T);
        }
    }
}

#endif
