using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public class NetworkPlayerData : INetworkSerializable
{
    public ulong ClientID => _clientID;
    public int ClientIndex => ClientIndex;
    public string PlayerName => PlayerName;

    private ulong _clientID;
    private int _clientIndex;
    private string _playerName;

    public NetworkPlayerData(ulong newClientID, int newClientIndex, string newPlayerName = null)
    {
        _clientID = newClientID;
        _clientIndex = newClientIndex;
        if (newPlayerName == null)
            _playerName = "UNDEFINED";
        else
            _playerName = newPlayerName;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _clientID);
        serializer.SerializeValue(ref _clientIndex);
        serializer.SerializeValue(ref _playerName);
    }
}
