#if NETCODE_PRESENT

using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IterationToolkit.Netcode
{

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

        //private static GameNetworkManager _instance;
        //public static GameNetworkManager Instance => Singleton.GetInstance<GameNetworkManager>(ref _instance);
        public static new GameNetworkManager Instance => SingletonManager.GetSingleton<GameNetworkManager>(typeof(GameNetworkManager));



        public int CurrentPlayerCount => PlayerNetworkObjects.Count;

        public NetworkComponentList<NetworkPlayerBase> PlayerNetworkObjects;

        public NetworkPlayerBase LocalPlayer => PlayerNetworkObjects.GetComponent(NetworkManagerInstance.LocalClient.PlayerObject);


        protected override void Awake()
        {
            base.Awake();
            PlayerNetworkObjects = new NetworkComponentList<NetworkPlayerBase>();
        }

        public override void OnNetworkSpawn()
        {
            TryAddAllPlayerObjectsServerRpc();
            OnClientGameNetworkManagerSpawnServerRpc(NetworkManagerInstance.LocalClientId);
        }

        public List<T> GetNetworkPlayers<T>() where T : NetworkPlayerBase
        {
            List<T> returnList = new List<T>();
            foreach (NetworkPlayerBase player in PlayerNetworkObjects.Components)
                returnList.Add(player as T);
            return (returnList);
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

        public void LoadScene(string sceneName)
        {
            LoadSceneServerRpc(sceneName, false);
        }

        public void LoadScene(LevelData levelData)
        {
            LoadSceneServerRpc(levelData.SceneName, false);
        }

        public void LoadSceneAdditive(LevelData levelData) //For UnityEvent Purposes
        {
            LoadSceneServerRpc(levelData.SceneName, true);
        }

        public void LoadScene(LevelData levelData, bool isAdditive)
        {
            LoadSceneServerRpc(levelData.SceneName, isAdditive);
        }

        [ServerRpc(RequireOwnership = false)]
        public void LoadSceneServerRpc(string sceneName, bool isAdditive = false)
        {
            NetworkManagerInstance.SceneManager.OnLoadComplete += OnNetworkLevelLoaded;
            if (isAdditive)
                NetworkManager.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            else
                NetworkManager.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        private void OnNetworkLevelLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
        {
            if (IsServer)
                OnNewLevelLoaded(SceneManager.GetSceneByName(sceneName), loadSceneMode, clientId);
        }


    }
}

#endif
