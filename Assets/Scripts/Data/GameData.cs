using System;
using System.Linq;
using System.Collections.Generic;

public class GameData
{
    public int GridSizeX;
    public int GridSizeY;
    public SkinTemplate Skin { get; }
    public TicTacToePlayer[] Players;

    public GameData(int gridSizeX, int gridSizeY, SkinTemplate skin)
    {
        GridSizeX = gridSizeX;
        GridSizeY = gridSizeY;

        var enums = Enum.GetValues(typeof(TicTacToeMark));
        var marks = new List<TicTacToeMark>();

        foreach (var item in enums)
        {
            var enumValue = (TicTacToeMark)item;
            if (enumValue != TicTacToeMark.None)
            {
                marks.Add(enumValue);
            }
        }

        Players = new TicTacToePlayer[marks.Count];
        for (int i = 0; i < marks.Count; i++)
        {
            Players[i] = new TicTacToePlayer(marks[i], $"Player {i + 1}");
        }
        Skin = skin;
    }
}
