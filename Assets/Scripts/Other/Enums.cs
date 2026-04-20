using System;

public enum InputState
{
    None = 0,
    MouseClick = 1,
    MouseDown = 2,
    MouseUp = 3,
}

public enum OpponentControl
{
    None = -1,
    Player = 0,
    AI = 1,
}


public enum WindowType
{
    None,
    MainMenu,
    GameMenu,
    HUD,
    GameOver,
}

public enum PopupType
{
    None,
    Settings,
    Stats,
    PreGame,
    AreYouSure,
}

public enum ScreenOrientation
{
    None,
    Portrait,
    Landscape
}