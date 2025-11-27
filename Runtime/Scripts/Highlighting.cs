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

    private static Camera ActiveCamera => OverrideCamera ? OverrideCamera : Camera.main;

    public static Camera OverrideCamera { get; set; }

    public static IHighlightable Highlighted { get; private set; }
    public static Transform HighlightedTransform { get; private set; }

    public static IListenOnlyEvent OnHighlightChanged => onHighlightsChanged;
    private static ExtendedEvent onHighlightsChanged = new ExtendedEvent();

    public static bool IsHighlighted(IHighlightable highlightable) => Highlighted != null && Highlighted == highlightable;

    public static Vector3 MousePoint => ActiveCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,ActiveCamera.nearClipPlane));

    //Public for projects that use this to debug/troubleshoot what's going on
    public static IEnumerable<RaycastHit> RecentResults { get; private set; }

    private static void Refresh()
    {
        if (ActiveCamera == null) return;
        (IHighlightable, Collider) closestHighlightable = default;
        //We Reverse because RaycastAll returns in order of first hit to last but the closest one to the camera should be the latest highlight
        RecentResults = Physics.RaycastAll(ActiveCamera.ScreenPointToRay(Input.mousePosition), Mathf.Infinity).Reverse();
        foreach (RaycastHit hit in RecentResults)
            if (hit.collider.TryGetComponent(out IHighlightable highlightable) && highlightable.IsHighlightable())
                closestHighlightable = (highlightable, hit.collider);

        if (Highlighted != null && closestHighlightable.Item1 == null) //If we previously had a highlight and now theres nothing to highlight
            Unhighlight();
        else if (Highlighted != null && closestHighlightable.Item1 != Highlighted) //if we previously had a highlight and now a closer highlightable was found
        {
            Unhighlight();
            Highlight(closestHighlightable.Item1, closestHighlightable.Item2.transform);
        }
        else if (Highlighted == null && closestHighlightable.Item1 != null) //If we didn't previously have a highlight and now there's something to highlight
            Highlight(closestHighlightable.Item1, closestHighlightable.Item2.transform);
    }

    private static void Unhighlight()
    {
        IHighlightable cache = Highlighted;
        Highlighted = null;
        HighlightedTransform = null;
        if (cache != null)
        {
            cache.OnHighlightChanged(false);
            onHighlightsChanged.Invoke();
        }
    }

    private static void Highlight(IHighlightable highlight, Transform source)
    {
        Highlighted = highlight;
        HighlightedTransform = source;
        highlight.OnHighlightChanged(true);
        onHighlightsChanged.Invoke();
    }

    protected class IHighlightsController : MonoBehaviour
    {
        private void FixedUpdate() => Refresh();
    }
}
