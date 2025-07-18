using IterationToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Highlighting
{
    private static IHighlightsController _instance;
    private static IHighlightsController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("IterationToolkit.IHighlightController").AddComponent<IHighlightsController>();
                GameObject.DontDestroyOnLoad(_instance.gameObject);
            }
            return (_instance);
        }
    }
    private static List<IHighlightable> trackingHighlights = new List<IHighlightable>();
    private static Dictionary<IHighlightable, bool> trackedHighlightStates = new Dictionary<IHighlightable, bool>();
    private static Dictionary<Transform, IHighlightable> highlightTransformDict = new Dictionary<Transform, IHighlightable>();
    private static HashSet<IHighlightable> currentlyMousedOverHighlights = new HashSet<IHighlightable>();

    private static HashSet<IHighlightable> highlightsThisFrame = new HashSet<IHighlightable>();

    public static Camera OverrideCamera { get; private set; }
    private static Camera ActiveCamera => OverrideCamera ? OverrideCamera : Camera.main;

    public static IHighlightable LatestHighlight { get; private set; }

    public static ExtendedEvent<IHighlightable> OnHighlight { get; private set; } = new ExtendedEvent<IHighlightable>();
    public static ExtendedEvent<IHighlightable> OnUnhighlight { get; private set; } = new ExtendedEvent<IHighlightable>();
    public static ExtendedEvent OnHighlightsChanged { get; private set; } = new ExtendedEvent();

    public static void Register(IHighlightable highlightable)
    {
        if (trackingHighlights.Contains(highlightable) || highlightable.Target == null) return;

        trackingHighlights.Add(highlightable);
        trackedHighlightStates.Add(highlightable, false);
        highlightTransformDict.Add(highlightable.Target.transform, highlightable);
        highlightable.Target.destroyCancellationToken.Register(() => Unregister(highlightable));
        Instance.enabled = true;
    }

    public static void Unregister(IHighlightable highlightable)
    {
        if (!trackingHighlights.Contains(highlightable)) return;

        trackingHighlights.Remove(highlightable);
        trackedHighlightStates.Remove(highlightable);
        highlightTransformDict.Remove(highlightable.Target.transform);
        if (currentlyMousedOverHighlights.Contains(highlightable)) currentlyMousedOverHighlights.Remove(highlightable);
        if (highlightsThisFrame.Contains(highlightable)) highlightsThisFrame.Remove(highlightable);

        if (_instance != null && trackingHighlights.Count == 0)
            _instance.enabled = false;
    }

    public static IHighlightable[] GetActiveHighlights() => highlightsThisFrame.ToArray();


    public static bool IsLastestHighlight(IHighlightable highlightable)
    {
        return (LatestHighlight == highlightable);
    }

    public static bool IsHighlighted(IHighlightable highlightable)
    {
        if (trackedHighlightStates.TryGetValue(highlightable, out bool state))
            return (state);
        return (false);
    }

    private static void Refresh()
    {
        highlightsThisFrame.Clear();

        foreach (RaycastHit hit in Physics.RaycastAll(ActiveCamera.ScreenPointToRay(Input.mousePosition), Mathf.Infinity))
            if (highlightTransformDict.TryGetValue(hit.collider.transform, out IHighlightable highlight))
                highlightsThisFrame.Add(highlight);

        foreach (IHighlightable registeredHighlight in trackingHighlights)
            ToggleHighlightState(registeredHighlight, highlightsThisFrame.Contains(registeredHighlight));
    }

    private static void ToggleHighlightState(IHighlightable highlightable, bool value)
    {
        if (trackedHighlightStates[highlightable] == value) return;
        trackedHighlightStates[highlightable] = value;
        if (value)
        {
            if (LatestHighlight != null && LatestHighlight != highlightable)
                ToggleHighlightState(LatestHighlight, false);
            LatestHighlight = highlightable;
            currentlyMousedOverHighlights.Add(highlightable);
            OnHighlight.Invoke(highlightable);
        }
        else
        {
            if (LatestHighlight == highlightable)
                LatestHighlight = null;
            currentlyMousedOverHighlights.Remove(highlightable);
            OnUnhighlight.Invoke(highlightable);
        }

        highlightable.OnHighlightChanged();
        OnHighlightsChanged.Invoke();
    }

    protected class IHighlightsController : MonoBehaviour
    {
        private void Awake() => enabled = trackingHighlights.Count > 0;
        private void FixedUpdate() => Refresh();
    }
}
