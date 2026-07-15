using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public interface IProfilerEntry
    {
        public string ReferenceName { get; }
        public string DisplayName { get; }
        public float CurrentValue { get; }

        public void Clear();
    }
}

