using System;
using System.Collections.Generic;

[Serializable]
public class UserStatsData
{
    public int Version = 1;
    public List<UserGameStatsEntry> Games = new List<UserGameStatsEntry>();
}

[Serializable]
public class UserGameStatsEntry
{
    public long PlayedAtUnixSeconds;
    public GameStatus GameStatus;
    public TicTacToeMark WinnerMark;
    public float MatchDurationSeconds;
    public int TotalMoves;
    public float Player1TimeSeconds;
    public float Player2TimeSeconds;
    public int Player1Moves;
    public int Player2Moves;
}

[Serializable]
public class UserStatsSummary
{
    public int TotalGames;
    public int Player1Wins;
    public int Player2Wins;
    public int TotalDraws;
    public float AverageMatchDurationSeconds;
    public float AverageMovesPerGame;
}
