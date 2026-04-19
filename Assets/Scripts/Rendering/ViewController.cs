using System;
using UnityEngine;
using System.Collections.Generic;

public class ViewController : MonoBehaviour
{
    public Camera MainCamera { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        MainCamera = Camera.main;
        SetCamera();
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

    }

    void OnPlayerViewChanged()
    {
        SetCamera();
    }

    void SetCamera()
    {

    }
}