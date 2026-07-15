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
        [RuntimeInitializeOnLoadMethod]
        private static void Start()
        {
            Application.quitting += Clear;
            AddEntry(new ProfilerFPSEntry("FPS"));
        }

        private static List<IProfilerEntry> activeProfilers = new List<IProfilerEntry>();

        private static Dictionary<string, IProfilerEntry> profileEntries = new Dictionary<string, IProfilerEntry>();

        private static void Clear()
        {
            foreach (string refName in profileEntries.Keys.ToList())
                RemoveEntry(refName);
        }

        public static void RegisterRecorder(ProfilerCategory category, string statName, string displayName)
        {
            AddEntry(new ProfilerRecorderEntry(category, statName, displayName));
        }

        private static void AddEntry(IProfilerEntry entry)
        {
            if (profileEntries.ContainsKey(entry.ReferenceName)) return;
            profileEntries[entry.ReferenceName] = entry;
        }

        private static void RemoveEntry(string referenceName)
        {
            if (!profileEntries.ContainsKey(referenceName)) return;
            profileEntries[referenceName].Clear();
            profileEntries.Remove(referenceName);
        }

        public static string GetProfilerResults()
        {
            string text = "DEBUG VALUES";

            foreach (IProfilerEntry entry in profileEntries.Values.OrderBy(e => e.CurrentValue))
                text += "\n" + (entry.DisplayName + ": ").Colorize(Color.yellow) + entry.CurrentValue.ToString("N0");

            return (text);
        }
    }
}
