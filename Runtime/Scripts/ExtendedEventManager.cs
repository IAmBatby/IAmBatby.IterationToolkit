using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendedEventManager
{
    private static List<ExtendedEvent> registeredEvents = new List<ExtendedEvent>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ClearEvents()
    {
        if (registeredEvents == null) return;
        int clearCount = 0;
        for (int i = registeredEvents.Count - 1; i > 0; i--)
        {
            if (registeredEvents[i] == null)
                registeredEvents.RemoveAt(i);
            else
            {
                clearCount++;
                registeredEvents[i].ClearListeners();
            }
        }
        Debug.Log("Cleared #" + clearCount + " Static ExtendedEvents.");
    }

    public static void RegisterExtendedEvent(ExtendedEvent extendedEvent)
    {
        if (extendedEvent == null) return;
        if (registeredEvents.Contains(extendedEvent)) return;

        registeredEvents.Add(extendedEvent);
    }
}
