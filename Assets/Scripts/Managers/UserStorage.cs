using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class UserStorage
{
    static readonly string StatsFilePath = Path.Combine(Application.persistentDataPath, Prefs.UserStatsPath);

    public static UserStatsData Load()
    {
        try
        {
            if (!File.Exists(StatsFilePath))
            {
                return new UserStatsData();
            }

            string json = File.ReadAllText(StatsFilePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new UserStatsData();
            }

            var data = JsonConvert.DeserializeObject<UserStatsData>(json);
            if (data == null)
            {
                return new UserStatsData();
            }

            if (data.Games == null)
            {
                data.Games = new System.Collections.Generic.List<UserGameStatsEntry>();
            }

            return data;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"UserStatsStorage Load failed: {ex.Message}");
            return new UserStatsData();
        }
    }

    public static void SaveGame(GameOverData gameOverData)
    {
        if (gameOverData == null || gameOverData.GameData == null)
        {
            return;
        }

        var data = Load();
        var entry = new UserGameStatsEntry
        {
            PlayedAtUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            GameStatus = gameOverData.GameStatus,
            WinnerMark = gameOverData.Winner != null ? gameOverData.Winner.Mark : TicTacToeMark.None,
            MatchDurationSeconds = gameOverData.MatchDuration,
            TotalMoves = gameOverData.TotalMoves
        };

        var players = gameOverData.GameData.Players;
        if (players != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];
                if (player == null)
                {
                    continue;
                }

                if (player.Mark == TicTacToeMark.X)
                {
                    entry.Player1TimeSeconds = player.AccumulatedTimeSeconds;
                    entry.Player1Moves = player.TotalMovesPlayed;
                }
                else if (player.Mark == TicTacToeMark.O)
                {
                    entry.Player2TimeSeconds = player.AccumulatedTimeSeconds;
                    entry.Player2Moves = player.TotalMovesPlayed;
                }
            }
        }

        data.Games.Add(entry);
        Save(data);
    }

    public static void Save(UserStatsData data)
    {
        try
        {
            string directory = Path.GetDirectoryName(StatsFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(StatsFilePath, json);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"UserStatsStorage Save failed: {ex.Message}");
        }
    }

    public static UserStatsSummary GetSummary()
    {
        var data = Load();
        var summary = new UserStatsSummary();
        if (data.Games == null || data.Games.Count == 0)
        {
            return summary;
        }

        summary.TotalGames = data.Games.Count;
        float totalDuration = 0f;
        int totalMoves = 0;

        for (int i = 0; i < data.Games.Count; i++)
        {
            var game = data.Games[i];
            totalDuration += game.MatchDurationSeconds;
            totalMoves += game.TotalMoves;

            if (game.GameStatus == GameStatus.Draw)
            {
                summary.TotalDraws++;
            }
            else if (game.GameStatus == GameStatus.Win)
            {
                if (game.WinnerMark == TicTacToeMark.X)
                {
                    summary.Player1Wins++;
                }
                else if (game.WinnerMark == TicTacToeMark.O)
                {
                    summary.Player2Wins++;
                }
            }
        }

        summary.AverageMatchDurationSeconds = totalDuration / summary.TotalGames;
        summary.AverageMovesPerGame = (float)totalMoves / summary.TotalGames;
        return summary;
    }
}
