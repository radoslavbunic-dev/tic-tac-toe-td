using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class UIMainMenu : UIWindow
{
    public Action OnPlayButtonClicked;
    public Action OnStatsButtonClicked;
    public Action OnSettingsButtonClicked;
    public Action OnExitButtonClicked;

    [SerializeField] Button playButton;
    [SerializeField] Button statsButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitButton;

    protected override void Start()
    {
        base.Start();
        GameEvents.PlayMusic(AudioClipsId.MenuMusic);
    }

    protected override void SetListeners()
    {
        playButton.onClick.SetListener(() =>
        {
            OnPlayButtonClicked?.Invoke();
        });
        exitButton.onClick.SetListener(() =>
        {
            OnExitButtonClicked?.Invoke();
        });
        statsButton.onClick.SetListener(() =>
        {
            OnStatsButtonClicked?.Invoke();
        });
        settingsButton.onClick.SetListener(() =>
        {
            OnSettingsButtonClicked?.Invoke();
        });
        base.SetListeners();
    }
}
