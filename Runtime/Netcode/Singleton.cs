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
            if (manager == null || (manager is T) == false)
                manager = (NetworkGlobalManager)Object.FindFirstObjectByType(typeof(T));
            return ((T)manager);
        }
    }
}

#endif
