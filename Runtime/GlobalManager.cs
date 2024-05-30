using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Playing, Paused }

[DefaultExecutionOrder(-10)]
public class GlobalManager : Manager
{
    public static GlobalManager Instance => Singleton<GlobalManager>.GetInstance(ref _manager);

    public GameState ActiveGameState { get; private set; }

    public LevelData ActiveLevel { get; private set; }
    public LevelData LoadingLevel { get; private set; }

    public ExtendedEvent<GameState> OnGameStateChange = new ExtendedEvent<GameState>();
    public ExtendedEvent<LevelData> OnBeforeLevelLoaded = new ExtendedEvent<LevelData>();
    public ExtendedEvent<LevelData> OnLevelLoaded = new ExtendedEvent<LevelData>();

    protected override void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        base.Awake();
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

    public void LoadNewLevel(LevelData newLevelData)
    {
        LoadingLevel = newLevelData;
        OnBeforeLevelLoaded.Invoke(LoadingLevel);
        SceneManager.sceneLoaded += OnNewLevelLoaded;
        SceneManager.LoadScene(newLevelData.SceneName);
    }

    private void OnNewLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == LoadingLevel.name)
        {
            SceneManager.sceneLoaded -= OnNewLevelLoaded;
            ActiveLevel = LoadingLevel;
            LoadingLevel = null;
            OnLevelLoaded.Invoke(ActiveLevel);
        }
    }
}
