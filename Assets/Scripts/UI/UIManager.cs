using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] CanvasGroup fadeCanvas;
    [SerializeField] float fadeOutDuration = 1;

    void Awake()
    {
        fadeCanvas.alpha = 1;
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnEnable()
    {
        BootstrapManager.OnInitialized += OnInitialized;
        ViewController.OnOrientationChanged += OnViewChanged;
        LoadingPlaySceneState.OnPlaySceneLoaded += OnPlaySceneLoaded;

    }

    protected virtual void OnDisable()
    {
        BootstrapManager.OnInitialized -= OnInitialized;
        ViewController.OnOrientationChanged -= OnViewChanged;
        LoadingPlaySceneState.OnPlaySceneLoaded -= OnPlaySceneLoaded;
    }

    void OnInitialized()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        var duration = fadeOutDuration;
        while (t < duration)
        {
            fadeCanvas.alpha = 1f - (t / duration);
            yield return t += Time.deltaTime;
        }

        fadeCanvas.alpha = 0;
        fadeCanvas.gameObject.SetActive(false);
        new MainMenuState().Enter(null);
    }

    void OnViewChanged(ScreenOrientation orientation)
    {
        if (orientation == ScreenOrientation.Portrait)
        {
            canvasScaler.matchWidthOrHeight = 0.5f;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0.0f;
        }
    }

    void OnPlaySceneLoaded()
    {
        new MainMenuState().Enter(null);
    }
}