using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;

public static class ProfilerManager
{
    [RuntimeInitializeOnLoadMethod] //Jank just triggers the singleton creation getter
    private static void Start() => Instance.enabled = true;

    private static ProfilerManagerController _instance;
    private static ProfilerManagerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("IterationToolkit.ProfilerManagerController").AddComponent<ProfilerManagerController>();
                GameObject.DontDestroyOnLoad(_instance.gameObject);
            }
            return (_instance);
        }
    }

    private static Dictionary<string, (int, string)> debugValues = new Dictionary<string, (int, string)>();
    private static List<(int, string)> sortedValues = new List<(int, string)>();

    private static List<(string, ProfilerRecorder)> activeProfilers = new List<(string, ProfilerRecorder)>();

    private static int framesPerSec;
    private static float frequency = 0.5f;

    public static void RegisterNewProfiler(ProfilerCategory category, string name)
    {
        AddNewProfiler(category, name);
    }

    private static ProfilerRecorder AddNewProfiler(ProfilerCategory category, string name)
    {
        ProfilerRecorder returnValue = ProfilerRecorder.StartNew(category, name);
        activeProfilers.Add((name, returnValue));
        return (returnValue);
    }

    private static void RefreshValue(string name, int value) => RefreshValue(name, value.ToString("N0"));
    private static void RefreshValue(string name, float value) => RefreshValue(name, value.ToString("N0"));
    private static void RefreshValue(string name, string value)
    {
        if (debugValues.ContainsKey(name))
            debugValues[name] = (debugValues[name].Item1, value);
        else
            debugValues.Add(name, (debugValues.Count, value));
    }

    private static void RefreshValues()
    {
        foreach ((string, ProfilerRecorder) rec in activeProfilers)
            RefreshValue(rec.Item1, rec.Item2.LastValue);
    }

    public static string GetProfilerText()
    {
        RefreshValues();

        string text = "DEBUG VALUES";
        sortedValues.Clear();
        foreach (KeyValuePair<string, (int, string)> kvp in debugValues)
            sortedValues.Add((kvp.Value.Item1, "\n" + (kvp.Key + ": ").Colorize(Color.yellow) + kvp.Value.Item2));

        foreach (string value in sortedValues.OrderBy(x => x.Item1).Select(x => x.Item2))
            text += value;

        return (text);
    }

    private static IEnumerator FPS()
    {
        for (; ; )
        {
            Vector2 prev = new(Time.frameCount, Time.realtimeSinceStartup);
            yield return new WaitForSeconds(frequency);

            framesPerSec = Mathf.RoundToInt((Time.frameCount - prev.x) / (Time.realtimeSinceStartup - prev.y));
            RefreshValue("FPS", framesPerSec);
        }
    }

    protected class ProfilerManagerController : MonoBehaviour
    {
        private void Awake() => RefreshValue("FPS", 0);

        private void Start() => StartCoroutine(FPS());

        private void OnDisable()
        {
            foreach ((string, ProfilerRecorder) rec in activeProfilers)
                rec.Item2.Dispose();
            activeProfilers.Clear();
        }
    }
}
