using System.Text.RegularExpressions;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    IState currentState;
    GameData gameData;
    TicTacToeMatch gameMatch;
    TicTacToePlayer activeTurnPlayer;

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
        activeTurnPlayer.AddTurnTime(Time.deltaTime);
    }


    void OnGameSceneLoaded(GameData newGame)
    {
        gameData = newGame;
        gameMatch = new TicTacToeMatch(gameData);
        activeTurnPlayer = null;

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
        float matchDuration = 0;
        int totalMoves = 0;
        for (int i = 0; i < gameData.Players.Length; i++)
        {
            var player = gameData.Players[i];
            totalMoves += player.TotalMovesPlayed;
            matchDuration += player.AccumulatedTimeSeconds;
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
