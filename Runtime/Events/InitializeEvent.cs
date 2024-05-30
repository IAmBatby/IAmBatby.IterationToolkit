using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InitializeEvent : MonoBehaviour
{
    public UnityEvent onAwakeEvent;
    public UnityEvent onStartEvent;
    public UnityEvent onGlobalManagerInitalizedEvent;

    public void Awake()
    {
        onAwakeEvent.Invoke();
    }

    public void Start()
    {
        onStartEvent.Invoke();
    }
}
