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
        [RuntimeInitializeOnLoadMethod] //Jank just triggers the singleton creation getter
        private static void Start()
        {
            Instance.enabled = true;
            UpdateFPS();
        }

        private static FPSTrackerController _instance;
        private static FPSTrackerController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("IterationToolkit.FPSTrackerController").AddComponent<FPSTrackerController>();
                    GameObject.DontDestroyOnLoad(_instance.gameObject);
                }
                return (_instance);
            }
        }

        public static int FrameRate { get; private set; } = 0;

        private static float frequency = 0.5f;

        private static IEnumerator FPS() //Move to async
        {
            for (; ; )
            {
                Vector2 prev = new(Time.frameCount, Time.realtimeSinceStartup);
                yield return new WaitForSeconds(frequency);
                FrameRate = Mathf.RoundToInt((Time.frameCount - prev.x) / (Time.realtimeSinceStartup - prev.y));
            }
        }

        private static async void UpdateFPS()
        {
            while (true)
            {
                Vector2 prev = new(Time.frameCount, Time.realtimeSinceStartup);
                await Task.Delay(TimeSpan.FromSeconds(frequency));
                FrameRate = Mathf.RoundToInt((Time.frameCount - prev.x) / (Time.realtimeSinceStartup - prev.y));
            }
        }

        protected class FPSTrackerController : MonoBehaviour
        {
            //private void Start() => StartCoroutine(FPS());
        }
    }
}
