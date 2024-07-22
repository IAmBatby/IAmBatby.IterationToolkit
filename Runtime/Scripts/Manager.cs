using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [DefaultExecutionOrder(-1)]
    public class Manager : MonoBehaviour
    {
        protected static Manager _manager;

        public ExtendedEvent OnInitalize = new ExtendedEvent();

        protected virtual void Awake()
        {
            OnInitalize.Invoke();
        }
    }
}
