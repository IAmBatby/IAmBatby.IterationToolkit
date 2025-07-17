using IterationToolkit;
using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;





#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif
public static class ExtendedEventManager
{
    private static Action onRefresh;

    static ExtendedEventManager() => LoadStaticEvents();

    private static void LoadStaticEvents()
    {
        onRefresh = null;
        List<(ExtendedEvent, FieldInfo)> foundStaticEvents = new List<(ExtendedEvent, FieldInfo)> ();
        foreach (MemberInfo member in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMembers()))
            if (member is FieldInfo field && field.IsStatic && field.FieldType == typeof(ExtendedEvent))
                foundStaticEvents.Add((field.GetValue(null) as ExtendedEvent, field));
        string log = "Managing Manual Domain Reloading For The Following ExtendedEvents:";
        foreach ((ExtendedEvent, FieldInfo) kvp in foundStaticEvents)
        {
            RegisterStaticEvent(kvp.Item1, kvp.Item2);
            log += "\n   >" + kvp.Item2.DeclaringType.Name + "." + kvp.Item2.Name;
        }
        if (foundStaticEvents.Count > 0)
            Debug.Log(log);
    }

    private static void RegisterStaticEvent(ExtendedEvent extendedEvent, FieldInfo field)
    {
        if (extendedEvent != null)
            onRefresh += () => extendedEvent.ClearListeners();
        else
            Debug.LogError("Static ExtendedEvent Found Via Reflection Is Null!");
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InvokeClear() => onRefresh?.Invoke();
}
