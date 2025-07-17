using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IterationToolkit;
using System.Reflection;
using System;
using System.Linq;
using UnityEditor;

[InitializeOnLoad]
public static class DomainReloadManager
{
    private static Action onRefresh;

    static DomainReloadManager() => LoadStaticEvents();

    private static void LoadStaticEvents()
    {
        onRefresh = null;
        List<(IDomainReloadable, string)> foundStaticEvents = new List<(IDomainReloadable, string)>();
        foreach (MemberInfo member in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMembers()))
        {
            if (member is FieldInfo field && field.IsStatic && typeof(IDomainReloadable).IsAssignableFrom(field.FieldType))
                foundStaticEvents.Add((field.GetValue(null) as IDomainReloadable, field.DeclaringType.Namespace + "." + field.DeclaringType.Name + "." + field.Name));
            else if (member is PropertyInfo property && property.GetGetMethod() != null && property.GetGetMethod().IsStatic && typeof(IDomainReloadable).IsAssignableFrom(property.PropertyType))
                foundStaticEvents.Add((property.GetValue(null) as IDomainReloadable, property.DeclaringType.Namespace + "." + property.DeclaringType.Name + "." + property.Name));
        }

        string log = "Managing Manual Domain Reloading For The Following IDomainReloadables:";
        foreach ((IDomainReloadable, string) kvp in foundStaticEvents)
        {
            RegisterDomainReloadable(kvp.Item1);
            if (kvp.Item2.StartsWith("."))
                log += "\n   >Assembly-CSharp" + kvp.Item2;
            else
                log += "\n   >" + kvp.Item2;
        }
        if (foundStaticEvents.Count > 0)
            Debug.Log(log);
    }

    private static void RegisterDomainReloadable(IDomainReloadable domainReloadable)
    {
        if (domainReloadable != null)
            onRefresh += () => domainReloadable.OnDomainRefresh();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InvokeClear() => onRefresh?.Invoke();
}
