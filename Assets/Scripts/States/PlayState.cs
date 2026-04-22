using System;
using UnityEngine;

public class PlayState : GameState
{
    public TicTacToePlayer CurrentPlayer { get; private set; }
    public bool IsWaitingForConfirmation { get; private set; }

    public PlayState(TicTacToePlayer activePlayer)
    {
        CurrentPlayer = activePlayer;
    }

    public override void Enter(IState fromState)
    {
        Board.OnBoardClicked += OnBoardClicked;
        GameEvents.OpenSettings += OpenSettings;
        GameEvents.OpenPlayScene += OpenPlayScene;
        base.Enter(fromState);
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenSettings();
            return;
        }
    }

    public override void Exit()
    {
        Board.OnBoardClicked -= OnBoardClicked;
        GameEvents.OpenSettings -= OpenSettings;
        GameEvents.OpenPlayScene -= OpenPlayScene;
        base.Exit();
    }

    void OpenSettings()
    {
        new GameMenuState().Enter(this);
    }

    void OpenPlayScene()
    {
        IsWaitingForConfirmation = true;
        var confirmationData = new ConfirmationData()
        {
            ConfirmCallback = new Action(() =>
            {
                new ForfeitState(CurrentPlayer).Enter(this);
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
            Message = "Game will end and you will lose",
            Duration = 10,
            Data = confirmationData
        });
    }


    void OnBoardClicked(TicTacToeCell cell)
    {
        if (IsWaitingForConfirmation)
        {
            return;
        }
        if (cell == null || CurrentPlayer == null || !cell.IsValidToPlaceMark())
        {
            return;
        }
        
        new MovePlayedState(CurrentPlayer, cell).Enter(this);
    }
}
