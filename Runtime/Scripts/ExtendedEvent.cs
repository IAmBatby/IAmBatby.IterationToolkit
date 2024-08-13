using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class ExtendedEvent
    {
        public delegate void Event();
        private event Event onEvent;
        public bool HasListeners => (Listeners != 0);
        public int Listeners { get; internal set; }
        public void Invoke() { onEvent?.Invoke(); }
        public void AddListener(Event listener) { onEvent += listener; Listeners++; }
        public void RemoveListener(Event listener) { onEvent -= listener; Listeners--; }
    }

    public class ExtendedEvent<T>
    {
        public delegate void ParameterEvent(T param);

        private event ParameterEvent onParameterEvent;
        private event Action onEvent;

        public bool HasListeners => (Listeners != 0);
        public int Listeners { get; internal set; }

        public void Invoke(T param) { onParameterEvent?.Invoke(param); onEvent?.Invoke(); }
        public void Invoke() { onEvent?.Invoke(); }

        public void AddListener(ParameterEvent listener) { onParameterEvent += listener; Listeners++; }
        public void AddListener(Action listener) { onEvent += listener; Listeners++; }

        public void RemoveListener(ParameterEvent listener) { onParameterEvent -= listener; Listeners--; }
        public void RemoveListener(Action listener) { onEvent -= listener; Listeners--; }
    }
}


