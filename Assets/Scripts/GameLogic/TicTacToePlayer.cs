public class TicTacToePlayer
{
    public TicTacToeMark Mark { get; }
    public string DisplayName { get; }
    public float AccumulatedTimeSeconds { get; private set; }
    public int TotalMovesPlayed { get; private set; }

    public TicTacToePlayer(TicTacToeMark mark, string displayName)
    {
        Mark = mark;
        DisplayName = displayName;
    }

    public void AddTurnTime(float deltaSeconds)
    {
        if (deltaSeconds <= 0f)
        {
            return;
        }

        AccumulatedTimeSeconds += deltaSeconds;
    }

    public void AddMovePlayed()
    {
        TotalMovesPlayed++;
    }
}
