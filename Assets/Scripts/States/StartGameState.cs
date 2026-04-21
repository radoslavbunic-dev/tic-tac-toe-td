using System;
using System.Collections;
using System.Collections.Generic;

public class StartGameState : GameState
{
    public GameData GameData { get; private set; }

    public StartGameState(GameData gameData)
    {
        GameData = gameData;
    }
}
