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

        public float Progress
        {
            get
            {
                if (coroutine != null)
                    return ((currentTimerLength - (startTime - Time.time)) - currentTimerLength);
                else
                    return (0f);
            }
        }

        public bool IsRunning => (coroutine != null);

        private float startTime;
        private float currentTimerLength;

        public ExtendedEvent onTimerStart = new ExtendedEvent();
        public ExtendedEvent onTimerEnd = new ExtendedEvent();

        public void StartTimer(MonoBehaviour host, float time)
        {
            coroutineHostBehaviour = host;
            if (coroutine == null)
            {
                coroutine = coroutineHostBehaviour.StartCoroutine(TimerCoroutine(time));
            }
        }

        private IEnumerator TimerCoroutine(float time)
        {
            currentTimerLength = time;
            startTime = Time.time;

            onTimerStart.Invoke();

            yield return new WaitForSeconds(time);

            currentTimerLength = 0f;
            startTime = 0f;

            onTimerEnd.Invoke();
            coroutine = null;
        }
    }
}
