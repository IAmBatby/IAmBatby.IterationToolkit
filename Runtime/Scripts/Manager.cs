using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [DefaultExecutionOrder(-1)]
    public class Manager : MonoBehaviour
    {
        public ExtendedEvent OnInitalize = new ExtendedEvent();

        public static Manager Instance => SingletonManager.GetSingleton<Manager>(typeof(Manager));

        protected virtual void Awake()
        {
            OnInitalize.Invoke();
        }
    }
}
