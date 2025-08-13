using IterationToolkit.Netcode;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
/*
[GenerateSerializationForTypeAttribute(typeof(ScriptableNetworkReference<>))]
public class NetworkedBase<T> : NetworkVariableBase where T : struct, INetworkSerializable
{
    public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta) { }
    public override void ReadField(FastBufferReader reader) { }
    public override void WriteDelta(FastBufferWriter writer) { }
    public override void WriteField(FastBufferWriter writer) { }
}
*/