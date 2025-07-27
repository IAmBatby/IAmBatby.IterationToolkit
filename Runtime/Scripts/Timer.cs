using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [System.Serializable]
    public class Timer
    {
        [SerializeField] private float lastResumedTime;
        [SerializeField] private float lastPausedTime = 0f;
        [SerializeField] private float startTime;
        [SerializeField] private float currentTimerLength;
        [SerializeField] private float cachedTime;
        private Coroutine coroutine;
        private MonoBehaviour coroutineHostBehaviour;
        private float ComparisonTime => IsPaused ? lastResumedTime : startTime;

        public float Progress => coroutine == null ? 0f : (currentTimerLength - (ComparisonTime - Time.time)) - currentTimerLength;
        public float TimeElapsed => IsPaused ? cachedTime : (Time.time - startTime) + cachedTime;
        public bool IsRunning => (coroutine != null);
        public bool IsPaused { get; private set; }

        public ExtendedEvent OnTimerStart { get; private set; } = new ExtendedEvent();
        public ExtendedEvent OnTimerFinish { get; private set; } = new ExtendedEvent();

        public Timer(MonoBehaviour host, float time, params Action[] onFinishCallbacks)
        {
            foreach (Action endCallback in onFinishCallbacks)
                OnTimerFinish.AddListener(endCallback);
            StartTimer(host, time);
        }

        public Timer(params Action[] onFinishCallbacks)
        {
            foreach (Action endCallback in onFinishCallbacks)
                OnTimerFinish.AddListener(endCallback);
        }

        public void Clear()
        {
            OnTimerStart = new ExtendedEvent();
            OnTimerFinish = new ExtendedEvent();
            TryStopTimer();
            lastResumedTime = 0f;
            lastPausedTime = 0f;
            startTime = 0f;
            currentTimerLength = 0f;
            cachedTime = 0f;

            OnClear();
        }

        protected virtual void OnClear() { }

        public void StartTimer(MonoBehaviour host, float time = Mathf.Infinity)
        {
            if (host == null)
            {
                Debug.LogError("Tried to start Timer but passed MonoBehaviour reference is null!");
                return;
            }    
            coroutineHostBehaviour = host;
            if (coroutine == null)
            {
                coroutine = coroutineHostBehaviour.StartCoroutine(TimerCoroutine(time));
            }
            else
                Debug.LogError("Cannot start Timer as Coroutine is not null!");
        }

        public void ToggleTimer(bool value)
        {
            if (IsRunning == false) return;

            IsPaused = value;

            if (value == true)
                cachedTime += Time.time - startTime;
            else
                startTime = Time.time;

            Debug.Log((IsPaused ? "Pausing" : "Unpausing") + " Timer! Elapsed Time Is: " + TimeElapsed);
        }

        public bool TryStopTimer()
        {
            if (coroutine != null)
                coroutineHostBehaviour.StopCoroutine(coroutine);
            coroutineHostBehaviour = null;
            coroutine = null;
            return (true);
        }

        private IEnumerator TimerCoroutine(float time)
        {
            currentTimerLength = time;
            startTime = Time.time;
            cachedTime = 0f;

            if (coroutineHostBehaviour != null)
                InvokeTimerEvent(OnTimerStart);

            yield return new WaitForSeconds(time);

            //I had these when I setup timer pausing but i think i want these off so i can still sample the finished progress
            //currentTimerLength = 0f;
            //startTime = 0f;

            coroutine = null;
            if (coroutineHostBehaviour != null)
                InvokeTimerEvent(OnTimerFinish);
        }

        protected virtual void InvokeTimerEvent(ExtendedEvent timerEvent)
        {
            timerEvent.Invoke();
        }
    }

    public class Timer<T> : Timer
    {
        public T Value { get; private set; }

        public new ExtendedEvent<T> OnTimerStart { get; private set; } = new ExtendedEvent<T>();
        public new ExtendedEvent<T> OnTimerFinish { get; private set; } = new ExtendedEvent<T>();

        public Timer(MonoBehaviour host, float time, T newValue, params Action<T>[] onFinishCallbacks)
        {
            Value = newValue;
            foreach (Action<T> endCallback in onFinishCallbacks)
                OnTimerFinish.AddListener(endCallback);
            StartTimer(host, time);
        }

        public Timer(T newValue, params Action<T>[] onFinishCallbacks)
        {
            Value = newValue;
            foreach (Action<T> endCallback in onFinishCallbacks)
                OnTimerFinish.AddListener(endCallback);
        }

        public Timer(T newValue)
        {
            Value = newValue;
        }

        protected override void InvokeTimerEvent(ExtendedEvent timerEvent)
        {
            if (timerEvent == base.OnTimerStart)
                OnTimerStart.Invoke(Value);
            else if (timerEvent == base.OnTimerFinish)
                OnTimerFinish.Invoke(Value);

        }

        protected override void OnClear()
        {
            Value = default(T);
            OnTimerStart = new ExtendedEvent<T>();
            OnTimerFinish = new ExtendedEvent<T>();
        }
    }
}
