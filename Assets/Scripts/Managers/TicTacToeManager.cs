using System;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    public static Action<TicTacToePlayer, TicTacToeCell> OnMovePlayed;
    public static Action<float> OnMatchDurationUpdated;

    IState currentState;
    GameData gameData;
    TicTacToeMatch gameMatch;
    TicTacToePlayer activeTurnPlayer;
    float matchDuration = 0;

    void OnEnable()
    {
        LoadingGameSceneState.OnGameSceneLoaded += OnGameSceneLoaded;
        GameState.OnStateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        LoadingGameSceneState.OnGameSceneLoaded -= OnGameSceneLoaded;
        GameState.OnStateChanged -= OnStateChanged;
    }

    void Update()
    {
        if (activeTurnPlayer == null)
        {
            return;
        }

        var dt = Time.deltaTime;
        matchDuration += dt;
        activeTurnPlayer.AddTurnTime(dt);
        OnMatchDurationUpdated?.Invoke(matchDuration);
    }

    void OnGameSceneLoaded(GameData newGame)
    {
        gameData = newGame;
        gameMatch = new TicTacToeMatch(gameData);
        activeTurnPlayer = null;
        matchDuration = 0;

        UIWindows.ShowWindow(new()
        {
            Type = WindowType.HUD,
            Callback = OnHUDLoaded
        });
    }

    void OnStateChanged(IState fromState, IState toState)
    {
        currentState = toState;
        if (currentState is MovePlayedState movePlayedState)
        {
            movePlayedState.Player.AddMovePlayed();
            CheckGameStatus(movePlayedState.Player, movePlayedState.Cell);
        }
        else if (currentState is ForfeitState forfeitState)
        {
            var losingPlayer = forfeitState.Player;
            var winMark = GetNextPlayerId(losingPlayer.Mark);
            EndGame(GameStatus.Win, GetPlayer(winMark));
        }
    }

    void OnHUDLoaded(UIWindow window)
    {
        var hud = (UIHUD)window;
        hud.Init(gameData);
        var startGameState = new StartGameState(gameData);
        startGameState.Enter(null);

        activeTurnPlayer = GetPlayer(TicTacToeMark.X);
        var playState = new PlayState(activeTurnPlayer);
        playState.Enter(startGameState);
    }

    void CheckGameStatus(TicTacToePlayer player, TicTacToeCell cell)
    {
        if (player.Mark != activeTurnPlayer.Mark)
        {
            return;
        }
        if (!gameMatch.IsEmpty(cell.Index))
        {
            return;
        }

        gameMatch.SetMark(cell.Index, player.Mark);
        OnMovePlayed?.Invoke(player, cell);

        if (gameMatch.TryGetWinner(out TicTacToeMark winMark, out var winningLine))
        {
            EndGame(GameStatus.Win, GetPlayer(winMark), winningLine);
            return;
        }
        else if (gameMatch.IsGridFull())
        {
            EndGame(GameStatus.Draw);
            return;
        }

        var nextPlayerId = GetNextPlayerId(player.Mark);
        activeTurnPlayer = GetPlayer(nextPlayerId);
        new PlayState(activeTurnPlayer).Enter(currentState);
    }

    void EndGame(GameStatus gameStatus, TicTacToePlayer winner = null, int[] winningLine = null)
    {
        int totalMoves = 0;
        for (int i = 0; i < gameData.Players.Length; i++)
        {
            var player = gameData.Players[i];
            totalMoves += player.TotalMovesPlayed;
        }

        var gameOverData = new GameOverData()
        {
            GameData = gameData,
            GameStatus = gameStatus,
            Winner = winner,
            WinningLine = winningLine,
            TotalMoves = totalMoves,
            MatchDuration = matchDuration
        };
        new GameOverState(gameOverData).Enter(currentState);
    }
    
    TicTacToeMark GetNextPlayerId(TicTacToeMark mark)
    {
        if (mark == TicTacToeMark.X)
        {
            return TicTacToeMark.O;
        }
        else if (mark == TicTacToeMark.O)
        {
            return TicTacToeMark.X;
        }
        return TicTacToeMark.None;
    }

    TicTacToePlayer GetPlayer(TicTacToeMark mark)
    {
        for (int i = 0; i < gameData.Players.Length; i++)
        {
            var player = gameData.Players[i];
            if (player.Mark == mark)
            {
                return player;
            }
        }
        return null;
    }
}
