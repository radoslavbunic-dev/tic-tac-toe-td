public class TicTacToeMatch
{
    public int TotalCells { get; protected set; }
    public TicTacToeMark[] Grid { get; protected set; }
    public int[][] WinLines { get; protected set; }

    public TicTacToeMatch(GameData gameData)
    {
        TotalCells = gameData.GridSizeX * gameData.GridSizeY;
        Grid = new TicTacToeMark[TotalCells];
        WinLines = GetWinLines();
        for (int i = 0; i < TotalCells; i++)
        {
            Grid[i] = TicTacToeMark.None;
        }
    }

    public TicTacToeMark GetMarkAt(int index)
    {
        if (index < 0 || index >= TotalCells)
        {
            return TicTacToeMark.None;
        }

        return Grid[index];
    }

    public bool IsEmpty(int index)
    {
        return index >= 0 && index < Grid.Length && Grid[index] == TicTacToeMark.None;
    }

    public void SetMark(int index, TicTacToeMark mark)
    {
        if (index < 0 || index >= TotalCells)
        {
            return;
        }

        Grid[index] = mark;
    }

    public bool IsGridFull()
    {
        for (int i = 0; i < TotalCells; i++)
        {
            if (Grid[i] == TicTacToeMark.None)
            {
                return false;
            }
        }

        return true;
    }

    public bool TryGetWinner(out TicTacToeMark winMark, out int[] line)
    {
        winMark = TicTacToeMark.None;
        line = null;

        for (int i = 0; i < WinLines.Length; i++)
        {
            int a = WinLines[i][0];
            int b = WinLines[i][1];
            int c = WinLines[i][2];
            if (Grid[a] == TicTacToeMark.None)
            {
                continue;
            }

            if (Grid[a] == Grid[b] && Grid[b] == Grid[c])
            {
                winMark = Grid[a];
                line = WinLines[i];
                return true;
            }
        }

        return false;
    }

    protected virtual int[][] GetWinLines()
    {
        return new int[][]
        {
            new[] { 0, 1, 2 },
            new[] { 3, 4, 5 },
            new[] { 6, 7, 8 },
            new[] { 0, 3, 6 },
            new[] { 1, 4, 7 },
            new[] { 2, 5, 8 },
            new[] { 0, 4, 8 },
            new[] { 2, 4, 6 }
        };
    }
}
