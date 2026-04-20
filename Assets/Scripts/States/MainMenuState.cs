
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : GameState
{
    public UIMainMenu Window { get; private set; }
    public bool IsWaitingForConfirmation { get; private set; }

    public override void Enter(IState fromState)
    {
        base.Enter(fromState);
        UIWindows.ShowWindow(new()
        {
            Type = WindowType.MainMenu,
            Callback = OnWindowLoaded
        });
    }

    void OnWindowLoaded(UIWindow window)
    {
        Window = (UIMainMenu)window;
        Window.OnPlayButtonClicked += OnPlayButtonClicked;
        Window.OnStatsButtonClicked += OnStatsButtonClicked;
        Window.OnSettingsButtonClicked += OnSettingsButtonClicked;
        Window.OnExitButtonClicked += OnExitButtonClicked;
    }

    void OnPlayButtonClicked()
    {
        if (IsWaitingForConfirmation)
        {
            return;
        }

        UIPopups.ShowPopup(new PopupData()
        {
            Type = PopupType.PreGame,
            Data = new PopupData()
            {
                SourceState = this,
            },
        });
    }

    void OnStatsButtonClicked()
    {
        if (IsWaitingForConfirmation)
        {
            return;
        }
        UIPopups.ShowPopup(new PopupData()
        {
            Type = PopupType.Stats,
        });
    }

    void OnSettingsButtonClicked()
    {
        if (IsWaitingForConfirmation)
        {
            return;
        }
        UIPopups.ShowPopup(new PopupData()
        {
            Type = PopupType.Settings,
        });
    }

    void OnExitButtonClicked()
    {
        if (IsWaitingForConfirmation)
        {
            return;
        }

        IsWaitingForConfirmation = true;
        var confirmationData = new ConfirmationData()
        {
            ConfirmCallback = new Action(() =>
            {
                Application.Quit();
            }),
            DeclineCallback = new Action(() =>
            {
                IsWaitingForConfirmation = false;
            }),
            ReverseButtons = true
        };
        UIPopups.ShowPopup(new PopupData()
        {
            Type = PopupType.AreYouSure,
            Title = "Are You Sure?",
            Message = "Game will exit",
            Duration = 10,
            Data = confirmationData
        });
    }

    public override void Exit()
    {
        if (Window)
        {
            Window.OnPlayButtonClicked -= OnPlayButtonClicked;
            Window.OnStatsButtonClicked -= OnStatsButtonClicked;
            Window.OnSettingsButtonClicked -= OnSettingsButtonClicked;
            Window.OnExitButtonClicked -= OnExitButtonClicked;
            Window = null;
        }
        base.Exit();
    }
}