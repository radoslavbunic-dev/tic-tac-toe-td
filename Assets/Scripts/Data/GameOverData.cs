using System;
using UnityEngine;

public class GameOverData
{
    public GameData GameData { get; set; }
    public GameStatus GameStatus { get; set; }
    public TicTacToePlayer Winner { get; set; }
    public int[] WinningLine { get; set; }
    public float MatchDuration { get; set; }
    public int TotalMoves { get; set; }
}