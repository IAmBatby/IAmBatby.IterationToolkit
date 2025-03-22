/*
#if NETCODE_PRESENT

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public class NetworkCountEvent : AnticipatedNetworkVariable<List<ulong>>
    {
        private int CurrentCount => Value.Count;
        private int MaxCount => NetworkManager.Singleton.ConnectedClients.Count;
        private Dictionary<ulong, bool> currentProgress = new Dictionary<ulong, bool>();

        public NetworkCountEvent(List<ulong> value = default, StaleDataHandling staleData = StaleDataHandling.Reanticipate) : base(value, staleData)
        {
            
        }

        public void CountClient(ulong clientID, bool value)
        {
            if (value == true && !AuthoritativeValue.Contains(clientID))
            {
                List<ulong> updatedList = new List<ulong>(AuthoritativeValue);
                updatedList.Add(clientID);
                Anticipate(updatedList);
            }    
        }
    }
}

#endif
*/
