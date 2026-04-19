using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState : IState, IDisposable
{
    public static Action<IState, IState> OnStateChanged;

    public IState FromState { get; protected set; }

    public virtual void Enter(IState fromState)
    {
        if (fromState != null)
        {
            fromState.Exit();
            if (fromState is GameState state && state.FromState is GameState stateToDispose)
            {
                stateToDispose.Dispose();
            }
        }

        string stateType = fromState != null ? fromState.GetType().ToString() : "none";
        Debug.LogWarning($"Enter State: {this} from: {stateType}");
        FromState = fromState;
        OnStateChanged?.Invoke(fromState, this);
    }

    public virtual void Execute() {}

    public virtual void Exit()
    {
#if UNITY_EDITOR
        Debug.LogWarning($"Exit State {this}");
#endif
    }

    public virtual void Dispose()
    {
        FromState = null;
    }
}