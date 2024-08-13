using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IterationToolkit
{
    public static class Singleton
    {
        public static T GetInstance<T>(ref Manager manager) where T : Manager
        {
            if (manager == null || (manager is T) == false)
                manager = (Manager)Object.FindFirstObjectByType(typeof(T));
            return ((T)manager);
        }
    }
}
