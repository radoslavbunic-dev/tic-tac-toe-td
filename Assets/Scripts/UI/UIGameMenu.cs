using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameMenu : UIWindow
{
    public Action OnCloseButtonClicked;

    [SerializeField] Button closeButton;

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetListeners()
    {
        closeButton.onClick.SetListener(() =>
        {
            OnCloseButtonClicked?.Invoke();
            Close();
        });
        base.SetListeners();
    }
}