using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Seeking Instance For: " + typeof(T).Name);
                _instance = Resources.LoadAll<T>(string.Empty)[0];
            }
            return (_instance);
        }
    }
}
