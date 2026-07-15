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
        public static float UpdateFrequency { get; set; } = 0.5f;

        private static bool exitEarly = false;
        private static bool isRunning = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static async void UpdateFPS()
        {
            if (isRunning) return;
            Debug.Log("Entering UpdateFPS");
            Application.quitting += () => exitEarly = true;
            while (true)
            {
                isRunning = true;
                Vector2 prev = new(Time.frameCount, Time.realtimeSinceStartup);
                await Task.Delay(TimeSpan.FromSeconds(UpdateFrequency));
                FrameRate = Mathf.RoundToInt((Time.frameCount - prev.x) / (Time.realtimeSinceStartup - prev.y));

                if (exitEarly)
                {
                    Debug.Log("Exiting UpdateFPS");
                    break;
                }
            }
        }
    }
}
