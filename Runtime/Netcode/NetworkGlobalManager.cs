#if NETCODE_PRESENT

using IterationToolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IterationToolkit.Netcode
{
    public abstract class NetworkGlobalManager : NetworkBehaviour
    {
        public static NetworkGlobalManager Instance => Singleton.GetInstance<NetworkGlobalManager>(ref _manager);
        protected static NetworkGlobalManager _manager;


        public GameState ActiveGameState { get; private set; }

        public LevelData ActiveLevel { get; private set; }
        public LevelData LoadingLevel { get; private set; }

        public ExtendedEvent OnInitalize = new ExtendedEvent();

        public ExtendedEvent<GameState> OnGameStateChange = new ExtendedEvent<GameState>();
        public ExtendedEvent<LevelData> OnBeforeLevelLoaded = new ExtendedEvent<LevelData>();
        public ExtendedEvent<LevelData> OnLevelLoaded = new ExtendedEvent<LevelData>();

        protected virtual void Awake()
        {
            GameObject.DontDestroyOnLoad(gameObject);
            OnInitalize.Invoke();
        }



        public void ChangeGameState(GameState newGameState)
        {
            ActiveGameState = newGameState;

            OnGameStateChanged();

            OnGameStateChange.Invoke(newGameState);
        }

        protected virtual void OnGameStateChanged()
        {

        }

        //Gotta refactor w/ networking in mind (cbf rn)
        /*
        public void LoadNewLevel(LevelData newLevelData)
        {
            LoadingLevel = newLevelData;
            OnBeforeLevelLoaded.Invoke(LoadingLevel);
            SceneManager.sceneLoaded += OnNewLevelLoaded;
            SceneManager.LoadScene(newLevelData.SceneName);
        }
        */

        protected void OnNewLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            if (LoadingLevel != null && scene.name == LoadingLevel.defaultSceneName)
            {
                ActiveLevel = LoadingLevel;
                LoadingLevel = null;
                OnLevelLoaded.Invoke(ActiveLevel);
            }
            else
                OnLevelLoaded.Invoke();
        }
    }
}

#endif
