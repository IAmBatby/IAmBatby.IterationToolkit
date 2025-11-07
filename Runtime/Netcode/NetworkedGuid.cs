#if NETCODE_PRESENT

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public class NetworkedGuid<T> : NetworkedBase<NetworkGuid> where T : ScriptableNetworkObject<T>
    {
        private ExtendedEvent<T> onValueChangedEvent = new ExtendedEvent<T>();
        public IListenOnlyEvent<T> OnValueChanged => onValueChangedEvent;

        public NetworkGuid NetworkGuid => m_InternalValue;
        public Guid Guid => m_InternalValue.ToGuid();

        public T Value
        {
            get => ScriptableNetworkObject<T>.Get(m_InternalValue);
            set
            {
                NetworkGuid guid = value.NetworkGuid;
                if (NetworkVariableSerialization<NetworkGuid>.AreEqual(ref m_InternalValue, ref guid))
                    return;

                NetworkBehaviour netBev = GetBehaviour();
                if (netBev && !CanClientWrite(netBev.NetworkManager.LocalClientId))
                    throw new InvalidOperationException("Client is not allowed to write to this NetworkVariable");

                Set(guid);
                m_IsDisposed = false;
            }
        }

        protected override void OnInvokeEvent()
        {
            onValueChangedEvent.Invoke(Value);
        }
    }
}

#endif