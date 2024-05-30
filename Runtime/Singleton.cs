using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Singleton<T> where T : Manager
{
    public static T GetInstance(ref Manager manager)
    {
        if (manager == null)
            manager = (Manager)(Object.FindAnyObjectByType(typeof(T)));

        return ((T)manager);
    }
}
