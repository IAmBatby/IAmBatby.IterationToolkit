using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit.Netcode
{
    [Serializable]
    public abstract class ScriptableNetworkObject : ScriptableObject
    {
        [NonSerialized] private static Dictionary<Guid, ScriptableNetworkObject> globalDict = new Dictionary<Guid, ScriptableNetworkObject>();
        [field: SerializeField] public ExtendedGuid ExtendedGuid { get; private set; }
        public Guid Guid => ExtendedGuid.Guid;
        public NetworkGuid NetworkGuid => ExtendedGuid.NetworkGuid;

        private void Awake() => TryRegister();
        private void OnEnable() => TryRegister();
        private void OnValidate() => TryRegister();

        protected virtual void TryRegister()
        {
            if (!globalDict.ContainsKey(Guid))
                globalDict.Add(Guid, this);
        }

        public bool GuidIsUnique => !ExtendedGuid.IsInvalid && globalDict.ContainsKey(Guid) && globalDict[Guid] == this;
    }

    [Serializable]
    public abstract class ScriptableNetworkObject<T> : ScriptableNetworkObject where T : ScriptableNetworkObject<T>
    {
        [NonSerialized] private static Dictionary<Guid, T> dict = new Dictionary<Guid, T>();

        protected override void TryRegister()
        {
            base.TryRegister();
            if (!dict.ContainsKey(ExtendedGuid.Guid))
                dict.Add(ExtendedGuid.Guid, this as T);
        }


        public static bool TryGetNetworkObject(NetworkGuid guid, out T result)
        {
            TryGetNetworkObject(guid.ToGuid(), out result);
            return (result != null);
        }
        public static bool TryGetNetworkObject(Guid guid, out T result)
        {
            result = null;
            if (dict.TryGetValue(guid, out T newResult))
                result = newResult;
            return (result != null);
        }
    }
}
