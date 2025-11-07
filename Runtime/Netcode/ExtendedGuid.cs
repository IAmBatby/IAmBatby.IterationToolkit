#if NETCODE_PRESENT

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [Serializable]
    public struct ExtendedGuid
    {
        [SerializeField] private byte[] m_Guid;
        public bool IsInvalid => m_Guid == null || m_Guid.Length == 0;


        public Guid Guid
        {
            get
            {
                if (IsInvalid) CreateGuid();
                return (new Guid(m_Guid));
            }
        }
        public NetworkGuid NetworkGuid => Guid.ToNetworkGuid();
    
        public ExtendedGuid(byte[] array = null)
        {
            m_Guid = array;
            CreateGuid();
        }

        private void CreateGuid()
        {
            if (Application.isPlaying) return;
            m_Guid = Guid.NewGuid().ToByteArray();
        }
    }
}

#endif