using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : GameState 
{
    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            new GameMenuState().Enter(this);
            return;
        }
    }
}