#if NETCODE_PRESENT

using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkGlobalManager : NetworkBehaviour
{
    protected static NetworkGlobalManager _manager;

    public ExtendedEvent OnInitalize = new ExtendedEvent();

    protected virtual void Awake()
    {
        OnInitalize.Invoke();
    }
}

#endif
