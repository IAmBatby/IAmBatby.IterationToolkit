using IterationToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    public struct ScriptableNetworkReference<T> : INetworkSerializable where T : ScriptableNetworkObject<T>
    {
        private NetworkGuid m_NetworkGuid;
        public bool IsInvalid => m_NetworkGuid.IsInvalid;


        public NetworkGuid NetworkGuid { get => m_NetworkGuid; set => m_NetworkGuid = value; }
        public Guid Guid => NetworkGuid.ToGuid();

        public ScriptableNetworkReference(T scriptableNetworkObject)
        {
            m_NetworkGuid = scriptableNetworkObject == null ? default : scriptableNetworkObject.NetworkGuid;
        }

        public bool TryGetObject(out T scriptableNetworkObject)
        {
            scriptableNetworkObject = Resolve(this);
            return (scriptableNetworkObject != null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T Resolve(ScriptableNetworkReference<T> reference)
        {
            if (ScriptableNetworkObject<T>.TryGetNetworkObject(reference.NetworkGuid, out T result))
                return (result);
            return (null);
        }

        public static implicit operator T(ScriptableNetworkReference<T> scriptableNetworkReference) => Resolve(scriptableNetworkReference);
        public static implicit operator ScriptableNetworkReference<T>(T scriptableNetworkObject) => new ScriptableNetworkReference<T>(scriptableNetworkObject);


        public void NetworkSerialize<M>(BufferSerializer<M> serializer) where M : IReaderWriter
        {
            serializer.SerializeValue(ref m_NetworkGuid);
        }
    }
}
