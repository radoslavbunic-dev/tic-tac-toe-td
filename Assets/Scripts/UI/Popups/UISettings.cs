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

    public override void Init(PopupData data)
    {
        base.Init(data);
        ApplyPrefsToToggles();
    }

    void ApplyPrefsToToggles()
    {
        bool musicOn = PlayerPrefs.GetInt(Prefs.MusicEnabled, 1) == 1;
        bool sfxOn = PlayerPrefs.GetInt(Prefs.SfxEnabled, 1) == 1;
        musicToggle.SetIsOnWithoutNotify(musicOn);
        sfxToggle.SetIsOnWithoutNotify(sfxOn);
    }

    protected override void SetListeners()
    {
        sfxToggle.onValueChanged.SetListener((value) =>
        {
            PlayerPrefs.SetInt(Prefs.SfxEnabled, value ? 1 : 0);
            PlayerPrefs.Save();
            GameEvents.SetSFXOnOff?.Invoke(value);
        });
        musicToggle.onValueChanged.SetListener((value) =>
        {
            PlayerPrefs.SetInt(Prefs.MusicEnabled, value ? 1 : 0);
            PlayerPrefs.Save();
            GameEvents.SetMusicOnOff?.Invoke(value);
        });
        base.SetListeners();
    }
}