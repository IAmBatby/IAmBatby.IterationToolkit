using IterationToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ExtendedEventManager
{
    private static List<ExtendedEvent> registeredEvents = new List<ExtendedEvent>();


    public static void AddEventListener(ExtendedEvent extendedEvent)
    {
        if (!registeredEvents.Contains(extendedEvent))
            registeredEvents.Add(extendedEvent);
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#endif
    private static void InvokeClear()
    {
        List<ExtendedEvent> cache = new List<ExtendedEvent>(registeredEvents);
        registeredEvents.Clear();
        int count = 0;  
        foreach (ExtendedEvent extendedEvent in cache)
        {
            if (extendedEvent.Listeners > 0)
            {
                extendedEvent.ClearListeners();
                count++;
            }

            AddEventListener(extendedEvent);
        }

        Debug.Log("Currently Tracking #" + registeredEvents.Count + " Events. Cleared Listeners On #" + count + " Events.");
    }

}
