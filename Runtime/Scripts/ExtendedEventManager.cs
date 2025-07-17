using IterationToolkit;
using System;
using UnityEngine;
using System.Linq;
using System.Reflection;




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
        foreach (MemberInfo member in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMembers()))
            if (member is FieldInfo field && field.IsStatic && field.FieldType == typeof(ExtendedEvent))
                RegisterStaticEvent(field.GetValue(null) as ExtendedEvent);
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
