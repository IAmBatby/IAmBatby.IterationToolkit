using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [System.Serializable]
    public class Timer
    {
        private Coroutine coroutine;
        private MonoBehaviour coroutineHostBehaviour;

        private float ComparisonTime => IsPaused ? lastResumedTime : startTime;

        public float Progress => coroutine == null ? 0f : (currentTimerLength - (ComparisonTime - Time.time)) - currentTimerLength;

        public float TimeElapsed
        {
            get
            {
                if (IsPaused)
                    return (cachedTime);
                else
                {
                    return (Time.time - (startTime + cachedTime));
                }
            }
        }

        public bool IsRunning => (coroutine != null);
        public bool IsPaused { get; private set; }

        [SerializeField] private float lastResumedTime;
        [SerializeField] private float lastPausedTime = 0f;
        [SerializeField] private float startTime;
        [SerializeField] private float currentTimerLength;

        [SerializeField] private float cachedTime;

        public ExtendedEvent onTimerStart = new ExtendedEvent();
        public ExtendedEvent onTimerEnd = new ExtendedEvent();

        public void StartTimer(MonoBehaviour host, float time = Mathf.Infinity)
        {
            coroutineHostBehaviour = host;
            if (coroutine == null)
            {
                coroutine = coroutineHostBehaviour.StartCoroutine(TimerCoroutine(time));
            }
        }

        public void ToggleTimer(bool value)
        {
            if (IsRunning == false) return;

            IsPaused = value;
            if (value == true)
                cachedTime = Time.time - (startTime + cachedTime);
        }

        public bool TryStopTimer()
        {
            if (coroutine == null)
                return (false);

            coroutineHostBehaviour.StopCoroutine(coroutine);
            coroutine = null;
            return (true);
        }

        private IEnumerator TimerCoroutine(float time)
        {
            currentTimerLength = time;
            startTime = Time.time;
            cachedTime = 0f;

            onTimerStart.Invoke();

            yield return new WaitForSeconds(time);

            currentTimerLength = 0f;
            startTime = 0f;

            onTimerEnd.Invoke();
            coroutine = null;
        }
    }
}
