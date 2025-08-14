using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public class Networked<T> : NetworkedBase<T>
    {
        private ExtendedEvent<T> onValueChangedEvent = new ExtendedEvent<T>();
        public IListenOnlyEvent<T> OnValueChanged => onValueChangedEvent;

        public T Value
        {
            get => m_InternalValue;
            set
            {
                if (NetworkVariableSerialization<T>.AreEqual(ref m_InternalValue, ref value))
                    return;

                NetworkBehaviour netBev = GetBehaviour();
                if (netBev && !CanClientWrite(netBev.NetworkManager.LocalClientId))
                    throw new InvalidOperationException("Client is not allowed to write to this NetworkVariable");

                Set(value);
                m_IsDisposed = false;
            }
        }

        protected override void OnInvokeEvent()
        {
            onValueChangedEvent.Invoke(Value);
        }


    }
}
