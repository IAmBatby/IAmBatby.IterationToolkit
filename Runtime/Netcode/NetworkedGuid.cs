using IterationToolkit;
using IterationToolkit.Netcode;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Serializable]
[GenerateSerializationForType(typeof(NetworkGuid))]
public class NetworkedGuid<T> : NetworkVariableBase where T : ScriptableNetworkObject<T>
{
    /*
    public T Reference { get => ScriptableNetworkObject<T>.Get(Value); set => Value = value.NetworkGuid; }
    private ExtendedEvent<T> onValueChangedEvent = new ExtendedEvent<T>();
    public IListenOnlyEvent<T> OnValueChange => onValueChangedEvent;

    public override void OnInitialize()
    {
        OnValueChanged += (NetworkGuid old, NetworkGuid newT) => onValueChangedEvent.Invoke(Reference);
        base.OnInitialize();
        onValueChangedEvent.Invoke(Reference); //Fire once on init for consistent reactive behaviour
        Debug.Log("On Value Changed Event!");
    }
    */

    private ExtendedEvent<T> onValueChangedEvent = new ExtendedEvent<T>();
    public IListenOnlyEvent<T> OnValueChanged => onValueChangedEvent;

    private NetworkBehaviour networkBehaviour;


    public delegate bool CheckExceedsDirtinessThresholdDelegate(in NetworkGuid previousValue, in NetworkGuid newValue);

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
        NetworkVariableSerialization<NetworkGuid>.Duplicate(m_InternalValue, ref m_PreviousValue);

        networkBehaviour = GetBehaviour();
        Debug.Log("NetBehaviour IsSpawned: " + networkBehaviour.IsSpawned + ", Value Is: " + Value);

        //onValueChangedEvent.Invoke(Value);
    }

    private IEnumerator WaitTillSpawn()
    {
        yield return new WaitUntil(() => networkBehaviour.IsSpawned);
    }

    public NetworkedGuid(NetworkGuid value = default, NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm) : base(readPerm, writePerm)
    {
        m_InternalValue = value;
        m_PreviousValue = default;
    }


    [SerializeField]
    private protected NetworkGuid m_InternalValue;
    private protected NetworkGuid m_PreviousValue;

    private bool m_HasPreviousValue;
    private bool m_IsDisposed;

    public virtual T Value
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

    internal ref NetworkGuid RefValue()
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

    ~NetworkedGuid()
    {
        Dispose();
    }

    public override bool IsDirty()
    {
        if (base.IsDirty())
            return true;

        var dirty = !NetworkVariableSerialization<NetworkGuid>.AreEqual(ref m_PreviousValue, ref m_InternalValue);
        SetDirty(dirty);
        return dirty;
    }


    public override void ResetDirty()
    {

        if (IsDirty())
        {
            m_HasPreviousValue = true;
            NetworkVariableSerialization<NetworkGuid>.Duplicate(m_InternalValue, ref m_PreviousValue);
        }
        base.ResetDirty();
    }

    private protected void Set(NetworkGuid value)
    {
        SetDirty(true);
        NetworkGuid previousValue = m_InternalValue;
        m_InternalValue = value;
        onValueChangedEvent.Invoke(Value);
    }

    public override void WriteDelta(FastBufferWriter writer)
    {
        NetworkVariableSerialization<NetworkGuid>.WriteDelta(writer, ref m_InternalValue, ref m_PreviousValue);
    }

  
    public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
    {
        NetworkGuid previousValue = m_InternalValue;
        NetworkVariableSerialization<NetworkGuid>.ReadDelta(reader, ref m_InternalValue);

        if (keepDirtyDelta)
            SetDirty(true);

        onValueChangedEvent.Invoke(Value);
    }

    public override void ReadField(FastBufferReader reader)
    {
        NetworkVariableSerialization<NetworkGuid>.Read(reader, ref m_InternalValue);
    }

    public override void WriteField(FastBufferWriter writer)
    {
        NetworkVariableSerialization<NetworkGuid>.Write(writer, ref m_InternalValue);
    }
}
