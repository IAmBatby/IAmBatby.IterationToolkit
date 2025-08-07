#if NETCODE_PRESENT

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public class NetworkPlayerBase : NetworkBehaviour
    {
        public NetworkPlayerBase LocalPlayer => GetLocalPlayer<NetworkPlayerBase>();

        public T GetLocalPlayer<T>() where T : NetworkPlayerBase
        {
            return GameNetworkManager.Instance.LocalPlayer as T;
        }
    }
}

#endif
