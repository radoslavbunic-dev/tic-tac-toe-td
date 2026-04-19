using System;
using System.Collections;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private IState currentState = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        GameState.OnStateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        GameState.OnStateChanged -= OnStateChanged;
    }

    void OnStateChanged(IState fromState, IState toState)
    {
        currentState = toState;
    }

    void Update()
    {
        currentState?.Execute();
    }
}
