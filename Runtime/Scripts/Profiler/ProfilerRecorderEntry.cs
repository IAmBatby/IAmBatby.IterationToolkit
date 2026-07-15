using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace IterationToolkit
{
    public struct ProfilerRecorderEntry : IProfilerEntry
    {
        public ProfilerRecorder Recorder { get; private set; }
        public string ReferenceName { get; private set; }
        public string DisplayName { get; private set; }
        public float CurrentValue => Recorder.LastValue;

        public ProfilerRecorderEntry(ProfilerCategory cat, string statName, string displayName)
        {
            ReferenceName = statName;
            DisplayName = displayName;
            Recorder = ProfilerRecorder.StartNew(cat, ReferenceName);
        }

        public void Clear() => Recorder.Dispose();
    }
}

