using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class ExtendedEvent : IExtendedEvent, IListenOnlyEvent
    {
        protected event Action onEvent;
        public bool HasListeners => (Listeners != 0);

        public virtual int Listeners => listeners.Count;
        private List<Action> listeners = new List<Action>();

        public ExtendedEvent() { DomainReloadManager.RegisterDomainReloadable(this); }
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
            onEvent = null;
            listeners.Clear();
        }

        public void OnDomainRefresh() => ClearListeners();
    }

    public class ExtendedEvent<T> : ExtendedEvent, IListenOnlyEvent<T>
    {
        public override int Listeners => base.Listeners + paramListeners.Count;
        private event Action<T> onParameterEvent;
        private List<Action<T>> paramListeners = new List<Action<T>>();


        public void Invoke(T param)
        {
            onParameterEvent?.Invoke(param);
            Invoke();
        }

        public void AddListener(Action<T> listener)
        {
            onParameterEvent += listener;
            paramListeners.Add(listener);
        }
        public void RemoveListener(Action<T> listener)
        {
            onParameterEvent -= listener;
            paramListeners.Remove(listener);
        }

        public override void ClearListeners()
        {
            base.ClearListeners();
            foreach (Action<T> listener in paramListeners)
                onParameterEvent -= listener;
            onParameterEvent = null;
            paramListeners.Clear();
        }
    }

    public class ExtendedEvent<T,U> : ExtendedEvent, IListenOnlyEvent<T,U>
    {
        public override int Listeners => base.Listeners + paramListeners.Count;
        private event Action<T,U> onParameterEvent;
        private List<Action<T,U>> paramListeners = new List<Action<T,U>>();


        public void Invoke(T firstParam, U secondParam)
        {
            onParameterEvent?.Invoke(firstParam,secondParam);
            Invoke();
        }

        public void AddListener(Action<T,U> listener)
        {
            onParameterEvent += listener;
            paramListeners.Add(listener);
        }
        public void RemoveListener(Action<T,U> listener)
        {
            onParameterEvent -= listener;
            paramListeners.Remove(listener);
        }

        public override void ClearListeners()
        {
            base.ClearListeners();
            foreach (Action<T,U> listener in paramListeners)
                onParameterEvent -= listener;
            onParameterEvent = null;
            paramListeners.Clear();
        }
    }
}


