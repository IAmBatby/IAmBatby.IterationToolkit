using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace IterationToolkit
{
    public struct ProfilerFPSEntry : IProfilerEntry
    {
        public string ReferenceName { get; private set; }
        public string DisplayName { get; private set; }
        public float CurrentValue => FPSTracker.FrameRate;

        public ProfilerFPSEntry(string displayName)
        {
            ReferenceName = "FPS";
            DisplayName = displayName;
        }

        public void Clear() { }
    }
}
