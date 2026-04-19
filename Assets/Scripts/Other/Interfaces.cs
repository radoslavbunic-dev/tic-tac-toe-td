using System;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter(IState fromState);
    void Execute();
    void Exit();
}

public interface IUIElement
{
    void OnClosed();
}

public interface IPersistentWindow
{
    WindowType Type { get; }
    HashSet<Type> OpenStates { get; }
}