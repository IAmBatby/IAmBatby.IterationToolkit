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

    [RuntimeInitializeOnLoadMethod] //Jank just triggers the singleton creation getter
    private static void Start() => Instance.enabled = true;

    private static HashSet<IHighlightable> highlighted = new HashSet<IHighlightable>();
    private static HashSet<IHighlightable> frameHighlighted = new HashSet<IHighlightable>();

    private static Camera ActiveCamera => OverrideCamera ? OverrideCamera : Camera.main;

    public static Camera OverrideCamera { get; private set; }

    public static IHighlightable LatestHighlight { get; private set; }
    public static Transform LatestHighlightTransform { get; private set; }

    public static ExtendedEvent OnHighlightsChanged { get; private set; } = new ExtendedEvent();

    public static IHighlightable[] GetActiveHighlights() => highlighted.ToArray();

    public static bool IsLastestHighlight(IHighlightable highlightable) => LatestHighlight == highlightable;
    public static bool IsHighlighted(IHighlightable highlightable) => highlighted.Contains(highlightable);

    private static void Refresh()
    {
        frameHighlighted.Clear();
        //We Reverse because RaycastAll returns in order of first hit to last but the closest one to the camera should be the latest highlight
        foreach (RaycastHit hit in Physics.RaycastAll(ActiveCamera.ScreenPointToRay(Input.mousePosition), Mathf.Infinity).Reverse())
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

    private static void ToggleHighlightState(IHighlightable highlightable, bool isHighlighted, Transform optionalTransform)
    {
        if (highlightable == null || highlighted.Contains(highlightable) == isHighlighted) return;
        if (isHighlighted)
        {
            if (LatestHighlight != null && LatestHighlight != highlightable) //If we are replacing the current latest highlight
                ToggleHighlightState(LatestHighlight, false, null);
            RefreshLatestHighlight(highlightable, optionalTransform);
            highlighted.Add(highlightable);
        }
        else
        {
            if (LatestHighlight == highlightable) //If we are turning off the latest highlight
                RefreshLatestHighlight(null, null);
            highlighted.Remove(highlightable);
        }

        highlightable.OnHighlightChanged(isHighlighted);
        OnHighlightsChanged.Invoke();
    }

    private static void RefreshLatestHighlight(IHighlightable highlight, Transform transform)
    {
        LatestHighlight = highlight;
        LatestHighlightTransform = transform;
    }

    protected class IHighlightsController : MonoBehaviour
    {
        private void FixedUpdate() => Refresh();
    }
}
