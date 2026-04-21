using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameSceneState : GameState
{
    public static Action<GameData> OnGameSceneLoaded;
    public GameData GameData { get; private set; }
    public bool IsWaitingForSceneToLoad { get; private set; }

    public LoadingGameSceneState(SkinTemplate skin, int gridSizeX = 3, int gridSizeY = 3)
    {
        GameData = new GameData(gridSizeX, gridSizeY, skin);
    }

    public override void Enter(IState fromState)
    {
        base.Enter(fromState);
        SceneManager.sceneLoaded += OnSceneLoaded;
        IsWaitingForSceneToLoad = true;
        SceneManager.LoadSceneAsync(Constants.GameScene, LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != Constants.GameScene)
        {
            return;
        }
        
        RemoveLoadingSceneListener();
        OnGameSceneLoaded?.Invoke(GameData);
    }

    public override void Exit()
    {
        RemoveLoadingSceneListener();
        base.Exit();
    }
    
    void RemoveLoadingSceneListener()
    {
        if (!IsWaitingForSceneToLoad)
        {
            return;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
        IsWaitingForSceneToLoad = false;
    }
}
