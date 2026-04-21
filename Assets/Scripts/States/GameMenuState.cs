using System;
using System.Collections;
using System.Collections.Generic;

public class GameMenuState : GameState
{
    public UISettings Settings { get; private set; }

    public override void Enter(IState fromState)
    {
        base.Enter(fromState);
        UIPopups.ShowPopup(new()
        {
            Type = PopupType.Settings,
            Callback = OnPopupLoaded
        });
    }

    void OnPopupLoaded(UIPopup popup)
    {
        Settings = (UISettings)popup;
        Settings.OnWindowClosed += OnSettingsClosed;
    }

    void OnSettingsClosed(UIWindow window)
    {
        FromState?.Enter(this);
    }
    
    public override void Exit()
    {
        if (Settings)
        {
            Settings.OnWindowClosed -= OnSettingsClosed;
            Settings = null;
        }
        base.Exit();
    }
}
