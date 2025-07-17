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
        Debug.Log("Looking For Static ExtendedEvents");
        onRefresh = null;
        List<(ExtendedEvent, string)> foundStaticEvents = new List<(ExtendedEvent, string)> ();
        foreach (MemberInfo member in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMembers()))
        {
            if (member is FieldInfo field && field.IsStatic && field.FieldType == typeof(ExtendedEvent))
                foundStaticEvents.Add((field.GetValue(null) as ExtendedEvent, field.DeclaringType.Name + "." + field.Name));
            else if (member is PropertyInfo property && property.GetGetMethod() != null && property.GetGetMethod().IsStatic && property.PropertyType == typeof(ExtendedEvent))
                foundStaticEvents.Add((property.GetValue(null) as ExtendedEvent, property.DeclaringType.Name + "." + property.Name));
        }

        string log = "Managing Manual Domain Reloading For The Following ExtendedEvents:";
        foreach ((ExtendedEvent, string) kvp in foundStaticEvents)
        {
            RegisterStaticEvent(kvp.Item1);
            log += "\n   >" + kvp.Item2;
        }
        if (foundStaticEvents.Count > 0)
            Debug.Log(log);
    }

    private static void RegisterStaticEvent(ExtendedEvent extendedEvent)
    {
        if (extendedEvent != null)
            onRefresh += () => extendedEvent.ClearListeners();
        else
            Debug.LogError("Static ExtendedEvent Found Via Reflection Is Null!");
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InvokeClear() => onRefresh?.Invoke();
}
