#if NETCODE_PRESENT

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{

    public class NetworkComponentList<M> : NetworkList<NetworkObjectReference> where M : MonoBehaviour
    {
        private Dictionary<NetworkObject, M> mainDictionary = new Dictionary<NetworkObject, M>();

        public List<M> Components { get; private set; } = new List<M>();

        private bool hasSynced;

        public NetworkComponentList(IEnumerable<NetworkObjectReference> values = default, NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm) : base(values, readPerm, writePerm)
        {
            OnListChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(NetworkListEvent<NetworkObjectReference> collectionInfo)
        {
            if (hasSynced == false)
                Sync();

            if (Contains(collectionInfo.Value) && !mainDictionary.ContainsKey(collectionInfo.Value)) //If it's new to us (being added)
                AddItem(collectionInfo.Value);
            else if (!Contains(collectionInfo.Value) && mainDictionary.ContainsKey(collectionInfo.Value)) //If it's not new to us (being removed)
                RemoveItem(collectionInfo.Value);
        }

        private void Sync()
        {
            foreach (NetworkObjectReference networkObjectReference in this)
                AddItem(networkObjectReference);
            hasSynced = true;
        }

        private void AddItem(NetworkObject networkObject)
        {
            networkObject.TryGetComponent(out M monoBehaviour);
            mainDictionary.Add(networkObject, monoBehaviour);
            Components.Add(monoBehaviour);
        }

        private void RemoveItem(NetworkObject networkObject)
        {
            mainDictionary.TryGetValue(networkObject, out M monoBehaviour);
            Components.Remove(monoBehaviour);
            mainDictionary.Remove(networkObject);
        }
    }
}

#endif
