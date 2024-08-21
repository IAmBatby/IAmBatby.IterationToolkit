#if NETCODE_PRESENT

using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public abstract class NetworkGlobalManager : NetworkBehaviour
    {
        public static NetworkGlobalManager Instance => Singleton.GetInstance<NetworkGlobalManager>(ref _manager);
        protected static NetworkGlobalManager _manager;

        public ExtendedEvent OnInitalize = new ExtendedEvent();

        protected virtual void Awake()
        {
            OnInitalize.Invoke();
        }
    }
}

#endif
