using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPlaySceneState : GameState
{
    public static Action OnPlaySceneLoaded;
    public bool IsWaitingForSceneToLoad { get; private set; }

    public override void Enter(IState fromState)
    {
        base.Enter(fromState);
        SceneManager.sceneLoaded += OnSceneLoaded;
        IsWaitingForSceneToLoad = true;
        SceneManager.LoadSceneAsync(Constants.PlayScene, LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != Constants.PlayScene)
        {
            return;
        }
        
        RemoveLoadingSceneListener();
        OnPlaySceneLoaded?.Invoke();
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
