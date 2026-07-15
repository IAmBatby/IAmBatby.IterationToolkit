using log4net.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IterationToolkit
{
    public static class FPSTracker
    {
        public static int FrameRate { get; private set; } = 0;
        private static float frequency = 0.5f;

        [RuntimeInitializeOnLoadMethod]
        private static void Start() => UpdateFPS();


        private static async void UpdateFPS()
        {
            while (true)
            {
                Vector2 prev = new(Time.frameCount, Time.realtimeSinceStartup);
                await Task.Delay(TimeSpan.FromSeconds(frequency));
                FrameRate = Mathf.RoundToInt((Time.frameCount - prev.x) / (Time.realtimeSinceStartup - prev.y));
            }
        }
    }
}
