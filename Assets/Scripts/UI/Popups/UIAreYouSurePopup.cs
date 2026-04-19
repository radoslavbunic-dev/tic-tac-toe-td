using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAreYouSurePopup : UIPopup
{
    public Action ConfirmCallback { get; protected set; }
    public Action DeclineCallback { get; protected set; }

    [SerializeField] protected TextMeshProUGUI titleText;
    [SerializeField] protected TextMeshProUGUI messageText;
    [SerializeField] protected Button confirmButton;

    public override void Init(PopupData data)
    {
        base.Init(data);
        var confirmationData = (ConfirmationData)data.Data;
        titleText.text = data.Title;
        messageText.text = data.Message;
        ConfirmCallback = confirmationData.ConfirmCallback;
        DeclineCallback = confirmationData.DeclineCallback;

        confirmButton.onClick.SetListener(() =>
        {
            ConfirmCallback?.Invoke();
            DeclineCallback = null;
            Close();
        });

        if (confirmationData.ReverseButtons)
        {
            closeButton.transform.SetSiblingIndex(0);
        }
    }

    public override void OnClosed()
    {
        DeclineCallback?.Invoke();
        base.OnClosed();
    }
}