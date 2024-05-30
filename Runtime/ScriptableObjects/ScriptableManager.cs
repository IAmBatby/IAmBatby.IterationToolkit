using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScriptableObjectSource { Editor, Runtime }

public static class ScriptableManager
{
    private static List<ScriptableObject> runtimeScriptableObjects = new List<ScriptableObject>();

    public static T Copy<T>(this T sourceObject) where T : ScriptableObject
    {
        T instancedScriptableObject = ScriptableObject.CreateInstance<T>();
        runtimeScriptableObjects.Add(instancedScriptableObject);
        return (instancedScriptableObject);
    }

    public static ScriptableObjectSource GetOrigin(this ScriptableObject so)
    {
        if (runtimeScriptableObjects.Contains(so))
            return (ScriptableObjectSource.Runtime);
        else
            return (ScriptableObjectSource.Editor);
    }

}
