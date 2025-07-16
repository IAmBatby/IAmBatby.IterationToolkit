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

    private static List<ExtendedEvent> cachedEvents = new List<ExtendedEvent>();

    public static void AddEventListener(ExtendedEvent extendedEvent)
    {
        if (!registeredEvents.Contains(extendedEvent))
            registeredEvents.Add(extendedEvent);
    }
    [InitializeOnLoadMethod]
    private static void InitializeOnLoad()
    {
        EditorApplication.playModeStateChanged -= OnEditorStateChange;
        EditorApplication.playModeStateChanged += OnEditorStateChange;
    }
    private static void OnEditorStateChange(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.ExitingEditMode) OnEditorEnteringPlay();
        else if (change == PlayModeStateChange.ExitingPlayMode) OnEditorExitingPlay();
    }

    private static void OnEditorExitingPlay()
    {
        //Debug.Log("On Editor Exiting!");
        registeredEvents = new List<ExtendedEvent>(cachedEvents);
    }

    private static void OnEditorEnteringPlay()
    {
        //Debug.Log("On Editor Playing!");
        cachedEvents = new List<ExtendedEvent>(registeredEvents);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InvokeClear()
    {
        List<ExtendedEvent> cache = new List<ExtendedEvent>(registeredEvents);
        //registeredEvents.Clear();
        int count = 0;  
        foreach (ExtendedEvent extendedEvent in cache)
        {
            if (extendedEvent.Listeners > 0)
            {
                extendedEvent.ClearListeners();
                count++;
            }

            //if (!Application.isPlaying)
            //AddEventListener(extendedEvent);
        }

        //Debug.Log("Currently Tracking #" + registeredEvents.Count + " Events. Cleared Listeners On #" + count + " Events.");
    }

}
