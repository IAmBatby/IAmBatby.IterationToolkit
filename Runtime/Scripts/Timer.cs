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

        private float ComparisonTime => IsPaused ? lastStartedTime : Time.time;

        public float Progress => coroutine == null ? 0f : (currentTimerLength - (startTime - ComparisonTime)) - currentTimerLength;

        public float TimeElapsed => ComparisonTime - startTime;

        public bool IsRunning => (coroutine != null);
        public bool IsPaused { get; private set; }

        [SerializeField] private float lastStartedTime;
        [SerializeField] private float startTime;
        [SerializeField] private float currentTimerLength;

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
            lastStartedTime = Time.time;
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
            lastStartedTime = Time.time;

            onTimerStart.Invoke();

            yield return new WaitForSeconds(time);

            currentTimerLength = 0f;
            startTime = 0f;

            onTimerEnd.Invoke();
            coroutine = null;
        }
    }
}
