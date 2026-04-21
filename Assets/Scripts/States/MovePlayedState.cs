using UnityEngine;

public class MovePlayedState : GameState
{
    public TicTacToePlayer Player { get; private set; }
    public TicTacToeCell Cell { get; private set; }

    public MovePlayedState(TicTacToePlayer player, TicTacToeCell cell)
    {
        Player = player;
        Cell = cell;
    }

    public override void Enter(IState fromState)
    {
        base.Enter(fromState);
        GameEvents.PlaySFX(AudioClipsId.PlayMove);
    }
}
