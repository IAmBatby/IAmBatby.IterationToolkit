using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public interface IExtendedEvent : IDomainReloadable
    {
        public bool HasListeners { get; }

        public void ClearListeners();

        public void Invoke();
    }
}
