using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHUD : UIWindow
{
    public static Action<UIWindow> OnMenuButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        SetListeners();
    }

    void SetPlayers()
    {
       
    }

    protected override void OnStateChanged(IState fromState, IState toState)
    {

    }

    protected override void SetListeners()
    {
        // menuButton.onClick.SetListener(() =>
        // {
        //     OnMenuButtonClicked?.Invoke(this);
        // });
        base.SetListeners();
    }
}