using IterationToolkit;
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

    public static Camera OverrideCamera { get; private set; }
    private static Camera ActiveCamera => OverrideCamera ? OverrideCamera : Camera.main;

    public static IHighlightable LatestHighlight { get; private set; }
    public static Transform LatestHighlightTransform { get; private set; }

    public static ExtendedEvent OnHighlightsChanged { get; private set; } = new ExtendedEvent();


    public static IHighlightable[] GetActiveHighlights() => highlighted.ToArray();

    [RuntimeInitializeOnLoadMethod]
    private static void Start() => Instance.enabled = true;

    public static bool IsLastestHighlight(IHighlightable highlightable) => LatestHighlight == highlightable;
    public static bool IsHighlighted(IHighlightable highlightable) => highlighted.Contains(highlightable);

    private static HashSet<IHighlightable> highlighted = new HashSet<IHighlightable>();
    private static HashSet<IHighlightable> frameHighlighted = new HashSet<IHighlightable>();
    private static void Refresh2()
    {
        frameHighlighted.Clear();
        foreach (RaycastHit hit in Physics.RaycastAll(ActiveCamera.ScreenPointToRay(Input.mousePosition), Mathf.Infinity))
            if (hit.collider.TryGetComponent(out IHighlightable highlightable))
            {
                frameHighlighted.Add(highlightable);
                if (!highlighted.Contains(highlightable))
                    ToggleHighlightState(highlightable, true, hit.transform);
            }

        foreach (IHighlightable higlightable in new HashSet<IHighlightable>(highlighted))
            if (!frameHighlighted.Contains(higlightable))
                ToggleHighlightState(higlightable, false, null);
    }

    private static void ToggleHighlightState(IHighlightable highlightable, bool value, Transform optionalTransform)
    {
        if (highlighted.Contains(highlightable) == value) return;
        if (value)
        {
            if (LatestHighlight != null && LatestHighlight != highlightable)
                ToggleHighlightState(LatestHighlight, false, null);
            LatestHighlightTransform = optionalTransform;
            LatestHighlight = highlightable;
            highlighted.Add(highlightable);
        }
        else
        {
            if (LatestHighlight == highlightable)
            {
                LatestHighlight = null;
                LatestHighlightTransform = null;
            }
            highlighted.Remove(highlightable);
        }

        highlightable.OnHighlightChanged(value);
        OnHighlightsChanged.Invoke();
    }

    protected class IHighlightsController : MonoBehaviour
    {
        private void FixedUpdate() => Refresh2();
    }
}
