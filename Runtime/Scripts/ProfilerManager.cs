using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;

namespace IterationToolkit
{
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

        private static int framesPerSec;
        private static float frequency = 0.5f;

        private static List<ProfilerEntry> activeProfilers = new List<ProfilerEntry>();




        public static void RegisterNewProfiler(ProfilerCategory category, string statName, string displayName)
        {
            activeProfilers.Add(new ProfilerEntry(category, statName, displayName));
        }

        public static string GetProfilerText()
        {
            string text = "DEBUG VALUES";

            foreach (ProfilerEntry entry in activeProfilers.OrderBy(e => e.LastValue))
                text += (entry.DisplayName + ": ").Colorize(Color.yellow) + entry.LastValue.ToString("N0");

            return (text);
        }

        private static IEnumerator FPS()
        {
            for (; ; )
            {
                Vector2 prev = new(Time.frameCount, Time.realtimeSinceStartup);
                yield return new WaitForSeconds(frequency);

                framesPerSec = Mathf.RoundToInt((Time.frameCount - prev.x) / (Time.realtimeSinceStartup - prev.y));
                //RefreshValue("FPS", framesPerSec);
            }
        }

        protected class ProfilerManagerController : MonoBehaviour
        {
            //private void Awake() => RefreshValue("FPS", 0);

            private void Start() => StartCoroutine(FPS());

            private void OnDisable()
            {
                foreach (ProfilerEntry rec in activeProfilers)
                    rec.Recorder.Dispose();
                activeProfilers.Clear();
            }
        }

        public struct ProfilerEntry
        {
            public ProfilerRecorder Recorder { get; private set; }
            public string StatName { get; private set; }
            public string DisplayName { get; private set; }
            public long LastValue => Recorder.LastValue;

            public ProfilerEntry(ProfilerCategory cat, string statName, string displayName)
            {
                StatName = statName;
                DisplayName = displayName;
                Recorder = ProfilerRecorder.StartNew(cat, StatName);
            }
        }
    }
}
