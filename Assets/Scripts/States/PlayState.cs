using System;
using UnityEngine;

public class PlayState : GameState
{
    readonly TicTacToePlayer currentPlayer;

    public PlayState(TicTacToePlayer activePlayer)
    {
        currentPlayer = activePlayer;
    }

    public override void Enter(IState fromState)
    {
        GameEvents.BoardClick += OnBoardClicked;
        base.Enter(fromState);
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            new GameMenuState().Enter(this);
            return;
        }
    }

    public override void Exit()
    {
        GameEvents.BoardClick -= OnBoardClicked;
        base.Exit();
    }

    void OnBoardClicked(TicTacToeCell cell)
    {
        if (cell == null || currentPlayer == null)
        {
            return;
        }
        
        new MovePlayedState(currentPlayer, cell).Enter(this);
    }
}
