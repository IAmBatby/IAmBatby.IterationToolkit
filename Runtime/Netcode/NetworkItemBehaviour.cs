#if NETCODE_PRESENT

using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public class NetworkItemBehaviour : NetworkBehaviour
    {
        public Rigidbody rigidBody;
        public NetworkItemData itemData;

        public bool IsItemActive { get; private set; }
        public bool isItemAvailable { get; private set; }

        public void ToggleItemActive(bool newValue)
        {
            IsItemActive = newValue;

            if (IsItemActive == true)
                OnItemActivated();
            else
                OnItemDectivated();
        }

        public void ToggleItemAvailable(bool newValue)
        {
            isItemAvailable = newValue;

            if (isItemAvailable == true)
                OnItemAvailable();
            else
                OnItemUnavailable();
        }

        protected virtual void OnItemActivated()
        {

        }

        protected virtual void OnItemDectivated()
        {

        }

        protected virtual void OnItemAvailable()
        {

        }

        protected virtual void OnItemUnavailable()
        {

        }
    }
}

#endif
