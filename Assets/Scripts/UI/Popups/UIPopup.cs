using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UIPopup : UIWindow
{
    [SerializeField] protected Button closeButton;

    public PopupData Data { get; protected set; }

    public virtual void Init(PopupData data)
    {
        Data = data;
        IsClosing = false;
        closeButton.onClick.SetListener(Close);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }
}
