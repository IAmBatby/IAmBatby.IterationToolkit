#if NETCODE_PRESENT

using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameNetworkManager : NetworkGlobalManager
{
    private static NetworkManager _networkManagerInstance;
    private static NetworkManager NetworkManagerInstance
    {
        get
        {
            if (_networkManagerInstance == null)
                _networkManagerInstance = FindObjectOfType<NetworkManager>();
            return _networkManagerInstance;
        }
    }

    private static GameNetworkManager _instance;
    public static GameNetworkManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameNetworkManager>();
            return _instance;
        }
    }



    public int CurrentPlayerCount => PlayerNetworkObjects.Count;

    public NetworkComponentList<NetworkPlayer> PlayerNetworkObjects;


    protected override void Awake()
    {
        base.Awake();
        PlayerNetworkObjects = new NetworkComponentList<NetworkPlayer>();
    }

    public override void OnNetworkSpawn()
    {
        TryAddAllPlayerObjectsServerRpc();
        OnClientGameNetworkManagerSpawnServerRpc(NetworkManagerInstance.LocalClientId);
    }

    public void StartHost()
    {
        if (NetworkManagerInstance.IsClient == false && NetworkManagerInstance.IsServer == false)
        {
            Debug.Log("Starting As Host");
            NetworkManagerInstance.StartHost();
        }
    }

    public void StartClient()
    {
        if (NetworkManagerInstance.IsClient == false && NetworkManagerInstance.IsServer == false)
        {
            Debug.Log("Starting As Client");
            NetworkManagerInstance.StartClient();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TryAddAllPlayerObjectsServerRpc()
    {
        foreach (NetworkClient networkClient in NetworkManager.ConnectedClientsList)
            if (networkClient.PlayerObject != null && !PlayerNetworkObjects.Contains(networkClient.PlayerObject))
                    PlayerNetworkObjects.Add(networkClient.PlayerObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnClientGameNetworkManagerSpawnServerRpc(ulong clientID)
    {
        SetOwnershipServerRpc(NetworkManagerInstance.ConnectedClients[clientID].PlayerObject, clientID);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetOwnershipServerRpc(NetworkObjectReference networkObjectReference, ulong clientID)
    {
        NetworkObject networkObject = (NetworkObject)networkObjectReference;
        networkObject.ChangeOwnership(clientID);
    }

    public void LoadScene(LevelData levelData, bool isAdditive = false)
    {
        LoadSceneServerRpc(levelData.SceneName, isAdditive);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoadSceneServerRpc(string sceneName, bool isAdditive = false)
    {
        if (isAdditive)
            NetworkManager.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        else
            NetworkManager.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }


}

#endif
