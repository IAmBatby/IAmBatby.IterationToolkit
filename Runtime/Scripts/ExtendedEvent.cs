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

        public virtual int Listeners => listeners.Count;
        private List<Action> listeners = new List<Action>();

        public ExtendedEvent() => ExtendedEventManager.AddEventListener(this);

        public void Invoke() { onEvent?.Invoke(); }

        public void AddListener(Action listener)
        { 
            onEvent += listener;
            listeners.Add(listener);
        }
        public void RemoveListener(Action listener)
        {
            onEvent -= listener;
            listeners.Remove(listener);
        }

        public virtual void ClearListeners()
        {
            foreach (Action listener in listeners)
                onEvent -= listener;
            listeners.Clear();
        }
    }

    public delegate void ParameterEvent<T>(T param);
    public class ExtendedEvent<T> : ExtendedEvent
    {
        public override int Listeners => base.Listeners + paramListeners.Count;
        private event ParameterEvent<T> onParameterEvent;
        private List<ParameterEvent<T>> paramListeners = new List<ParameterEvent<T>>();

        public ExtendedEvent() => ExtendedEventManager.AddEventListener(this);

        public void Invoke(T param)
        {
            onParameterEvent?.Invoke(param);
            Invoke();
        }

        public void AddListener(ParameterEvent<T> listener)
        {
            onParameterEvent += listener;
            paramListeners.Add(listener);
        }
        public void RemoveListener(ParameterEvent<T> listener)
        {
            onParameterEvent -= listener;
            paramListeners.Remove(listener);
        }

        public override void ClearListeners()
        {
            base.ClearListeners();
            foreach (ParameterEvent<T> listener in paramListeners)
                onParameterEvent -= listener;
            paramListeners.Clear();
        }
    }
}


