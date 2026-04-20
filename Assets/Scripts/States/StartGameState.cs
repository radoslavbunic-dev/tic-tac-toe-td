using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameState : GameState
{
    public Action OnGameStarted;
    public SkinTemplate Skin { get; private set; }
    public UIHUD Window { get; private set; }

    public StartGameState(SkinTemplate skin)
    {
        Skin = skin;
    }

    public override void Enter(IState fromState)
    {
        base.Enter(fromState);

        GameSession.SelectedSkin = Skin;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(Constants.GameScene, LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != Constants.GameScene)
        {
            return;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
        UIWindows.ShowWindow(new()
        {
            Type = WindowType.HUD,
            Callback = OnWindowLoaded
        });
    }

    void OnWindowLoaded(UIWindow window)
    {
        Window = (UIHUD)window;
        OnGameStarted?.Invoke();
        new PlayState().Enter(this);
    }

    public override void Exit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.Exit();
    }
}