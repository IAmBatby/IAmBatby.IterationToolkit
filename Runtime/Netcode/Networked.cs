using IterationToolkit.Netcode;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/*
public class Networked<T> : NetworkedBase<ScriptableNetworkReference<T>> where T : ScriptableNetworkObject<T>
{
    private ScriptableNetworkReference<T> m_NetworkReference;
    public T Value { get => m_NetworkReference as T; set => m_NetworkReference = value; }

    public override void ReadField(FastBufferReader reader)
    {
        NetworkVariableSerialization<ScriptableNetworkReference<T>>.Read(reader, ref m_NetworkReference);
    }

    public override void WriteField(FastBufferWriter writer)
    {
        NetworkVariableSerialization<ScriptableNetworkReference<T>>.Write(writer, ref m_NetworkReference);
    }
}
*/