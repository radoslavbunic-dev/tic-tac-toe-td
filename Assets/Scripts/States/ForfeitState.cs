using UnityEngine;

public class ForfeitState : GameState
{
    public TicTacToePlayer Player { get; private set; }

    public ForfeitState(TicTacToePlayer player)
    {
        Player = player;
    }
}
