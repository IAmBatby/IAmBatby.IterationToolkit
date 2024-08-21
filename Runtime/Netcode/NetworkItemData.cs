#if NETCODE_PRESENT

using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public class NetworkItemData : ItemData
    {
        public new NetworkItemBehaviour SpawnPrefab()
        {
            GameObject instancedItemPrefab = GameObject.Instantiate(itemPrefab);
            return (instancedItemPrefab.GetComponent<NetworkItemBehaviour>());
        }
    }
}

#endif
