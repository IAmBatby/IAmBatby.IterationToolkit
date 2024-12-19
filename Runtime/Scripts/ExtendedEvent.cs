using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class ExtendedEvent
    {
        protected event Action onEvent;
        public bool HasListeners => (Listeners != 0);

        public int Listeners { get; internal set; }

        public void Invoke() { onEvent?.Invoke(); }

        public void AddListener(Action listener) { onEvent += listener; Listeners++; }
        public void RemoveListener(Action listener) { onEvent -= listener; Listeners--; }
    }

    public delegate void ParameterEvent<T>(T param);
    public class ExtendedEvent<T> : ExtendedEvent
    {
        private event ParameterEvent<T> onParameterEvent;

        public void Invoke(T param)
        {
            onParameterEvent?.Invoke(param);
            Invoke();
        }

        public void AddListener(ParameterEvent<T> listener) { onParameterEvent += listener; Listeners++; }
        public void RemoveListener(ParameterEvent<T> listener) { onParameterEvent -= listener; Listeners--; }
    }
}


