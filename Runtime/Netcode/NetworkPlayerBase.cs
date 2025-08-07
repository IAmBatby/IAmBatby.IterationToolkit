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
        public int ClientId => (int)_clientId.Value;
        [SerializeField] private NetworkVariable<ulong> _clientId = new NetworkVariable<ulong>();
        public NetworkPlayerBase LocalPlayer => GetLocalPlayer<NetworkPlayerBase>();

        public T GetLocalPlayer<T>() where T : NetworkPlayerBase
        {
            return GameNetworkManager.Instance.LocalPlayer as T;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer) _clientId.Value = NetworkManager.ConnectedClients.Values.Where(c => c.PlayerObject == this).FirstOrDefault().ClientId;
            base.OnNetworkSpawn();
        }
    }
}

#endif
