using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerEvent : MonoBehaviour
{
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerFinish;
    public UnityEvent OnTimerClear;

    [SerializeField] public float defaultTime;
    private Timer timer;

    public void StartTimerEvent(float time = 0f)
    {
        if (time == 0f)
            time = defaultTime;


        if (timer != null)
        {
            timer.TryStopTimer();
            timer = null;
            OnTimerClear.Invoke();
        }

        timer = new Timer();
        timer.OnTimerStart.AddListener(OnTimerStart.Invoke);
        timer.OnTimerFinish.AddListener(OnTimerFinish.Invoke);
        timer.StartTimer(this, time);
    }
}
