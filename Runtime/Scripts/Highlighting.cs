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

    public static Camera ActiveCamera => OverrideCamera ? OverrideCamera : Camera.main;

    public static Camera OverrideCamera { get; set; }

    public static float ActiveRaycastDistance => OverrideRaycastDistance == -1f ? Mathf.Infinity : OverrideRaycastDistance;
    public static float OverrideRaycastDistance { get; set; } = -1f;

    public enum RayMode { Screen, Direction }
    public static RayMode ActiveRayMode { get; set; }

    public enum TargetMode { Collider, Rigidbody }
    public static TargetMode ActiveTargetMode { get; set; }

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
        (IHighlightable, Transform) closestHighlightable = default;
        //We Reverse because RaycastAll returns in order of first hit to last but the closest one to the camera should be the latest highlight
        RecentResults = Physics.RaycastAll(GetRay(), ActiveRaycastDistance).Reverse();
        foreach (RaycastHit hit in RecentResults)
        {
            Transform target = null;
            if (ActiveTargetMode == TargetMode.Collider)
                target = hit.collider.transform;
            else if (ActiveTargetMode == TargetMode.Rigidbody && hit.rigidbody != null)
                target = hit.rigidbody.transform;
            if (target == null) continue;
            if (target.TryGetComponent(out IHighlightable highlightable) && highlightable.IsHighlightable())
                closestHighlightable = (highlightable, target);
        }

        if (Highlighted != null && closestHighlightable.Item1 == null) //If we previously had a highlight and now theres nothing to highlight
            Unhighlight();
        else if (Highlighted != null && closestHighlightable.Item1 != Highlighted) //if we previously had a highlight and now a closer highlightable was found
        {
            Unhighlight();
            Highlight(closestHighlightable.Item1, closestHighlightable.Item2);
        }
        else if (Highlighted == null && closestHighlightable.Item1 != null) //If we didn't previously have a highlight and now there's something to highlight
            Highlight(closestHighlightable.Item1, closestHighlightable.Item2);
    }
    
    public static Ray GetRay()
    {
        if (ActiveRayMode == RayMode.Direction)
            return (new Ray(ActiveCamera.transform.position, ActiveCamera.transform.forward));
        else
            return (ActiveCamera.ScreenPointToRay(Input.mousePosition));
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
