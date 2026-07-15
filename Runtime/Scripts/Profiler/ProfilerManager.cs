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
            activeProfilers.Add(new ProfilerFPSEntry("FPS"));
        }

        private static List<IProfilerEntry> activeProfilers = new List<IProfilerEntry>();

        private static void Clear()
        {
            foreach (IProfilerEntry rec in activeProfilers)
                rec.Clear();
            activeProfilers.Clear();
        }

        public static void RegisterRecorder(ProfilerCategory category, string statName, string displayName)
        {
            activeProfilers.Add(new ProfilerRecorderEntry(category, statName, displayName));
        }

        public static string GetProfilerResults()
        {
            string text = "DEBUG VALUES";

            foreach (IProfilerEntry entry in activeProfilers.OrderBy(e => e.CurrentValue))
                text += "\n" + (entry.DisplayName + ": ").Colorize(Color.yellow) + entry.CurrentValue.ToString("N0");

            return (text);
        }
    }
}
