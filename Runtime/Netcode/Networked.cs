using IterationToolkit.Netcode;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    //Basicially a 1:1 remake of NGO's NetworkVariable but with three notable changes.
    // 1. Uses IterationToolkit's ExtendedEvents for ValueChanged action which I prefer
    // 2. ValueChanged action fires on Initialization for consistent reactive behaviour on clients joining pre-existing lobbies
    // 3. Allows for slightly shorter decleration names which is kinda nice.

    [Serializable]
    [GenerateSerializationForGenericParameter(0)]
    public class Networked<T> : NetworkVariableBase
    {
        private ExtendedEvent<T> onValueChangedEvent = new ExtendedEvent<T>();
        public IListenOnlyEvent<T> OnValueChanged => onValueChangedEvent;

        private NetworkBehaviour networkBehaviour;


        public delegate bool CheckExceedsDirtinessThresholdDelegate(in T previousValue, in T newValue);

        public CheckExceedsDirtinessThresholdDelegate CheckExceedsDirtinessThreshold;

        public override bool ExceedsDirtinessThreshold()
        {
            if (CheckExceedsDirtinessThreshold != null && m_HasPreviousValue)
                return CheckExceedsDirtinessThreshold(m_PreviousValue, m_InternalValue);
            return true;
        }

        private void OnInitialSync()
        {

        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            m_HasPreviousValue = true;
            NetworkVariableSerialization<T>.Duplicate(m_InternalValue, ref m_PreviousValue);

            networkBehaviour = GetBehaviour();
            Debug.Log("NetBehaviour IsSpawned: " + networkBehaviour.IsSpawned + ", Value Is: " + Value);

            //onValueChangedEvent.Invoke(Value);
        }

        public Networked(T value = default, NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm) : base(readPerm, writePerm)
        {
            m_InternalValue = value;
            m_PreviousValue = default;
        }


        [SerializeField]
        private protected T m_InternalValue;
        private protected T m_PreviousValue;

        private bool m_HasPreviousValue;
        private bool m_IsDisposed;

        public virtual T Value
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

        internal ref T RefValue()
        {
            return ref m_InternalValue;
        }

        public override void Dispose()
        {
            if (m_IsDisposed)
            {
                return;
            }

            m_IsDisposed = true;
            if (m_InternalValue is IDisposable internalValueDisposable)
            {
                internalValueDisposable.Dispose();
            }

            m_InternalValue = default;
            if (m_HasPreviousValue && m_PreviousValue is IDisposable previousValueDisposable)
            {
                m_HasPreviousValue = false;
                previousValueDisposable.Dispose();
            }

            m_PreviousValue = default;
        }

        ~Networked()
        {
            Dispose();
        }

        public override bool IsDirty()
        {
            if (base.IsDirty())
                return true;

            var dirty = !NetworkVariableSerialization<T>.AreEqual(ref m_PreviousValue, ref m_InternalValue);
            SetDirty(dirty);
            return dirty;
        }


        public override void ResetDirty()
        {

            if (IsDirty())
            {
                m_HasPreviousValue = true;
                NetworkVariableSerialization<T>.Duplicate(m_InternalValue, ref m_PreviousValue);
            }
            base.ResetDirty();
        }

        private protected void Set(T value)
        {
            SetDirty(true);
            T previousValue = m_InternalValue;
            m_InternalValue = value;
            onValueChangedEvent.Invoke(Value);
        }

        public override void WriteDelta(FastBufferWriter writer)
        {
            NetworkVariableSerialization<T>.WriteDelta(writer, ref m_InternalValue, ref m_PreviousValue);
        }


        public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
        {
            T previousValue = m_InternalValue;
            NetworkVariableSerialization<T>.ReadDelta(reader, ref m_InternalValue);

            if (keepDirtyDelta)
                SetDirty(true);

            onValueChangedEvent.Invoke(Value);
        }

        public override void ReadField(FastBufferReader reader)
        {
            NetworkVariableSerialization<T>.Read(reader, ref m_InternalValue);
        }

        public override void WriteField(FastBufferWriter writer)
        {
            NetworkVariableSerialization<T>.Write(writer, ref m_InternalValue);
        }
    }
}
