using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISettings : UIPopup
{
    [SerializeField] protected Toggle sfxToggle;
    [SerializeField] protected Toggle musicToggle;

    protected override void SetListeners()
    {
        sfxToggle.onValueChanged.SetListener((value) =>
        {
            // GameSettings.Instance.SFXVolume = value ? 1 : 0;
        });
        musicToggle.onValueChanged.SetListener((value) =>
        {
            // GameSettings.Instance.MusicVolume = value ? 1 : 0;
        });
        base.SetListeners();
    }
}