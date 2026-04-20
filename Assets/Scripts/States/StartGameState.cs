using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}