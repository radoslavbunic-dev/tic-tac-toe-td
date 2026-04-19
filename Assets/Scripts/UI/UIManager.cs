using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager : MonoBehaviour
{
    public static event Action<ScreenOrientation> OnOrientationChanged;
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] CanvasGroup fadeCanvas;
    [SerializeField] float fadeOutDuration = 1;
    ScreenOrientation screenOrientation;

    void Awake()
    {
        fadeCanvas.alpha = 1;
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnEnable()
    {
        BootstrapManager.OnInitialized += OnInitialized;
    }

    protected virtual void OnDisable()
    {
        BootstrapManager.OnInitialized -= OnInitialized;
    }

    void OnInitialized()
    {
        screenOrientation = GetScreenOrientation();
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

    void Update()
    {
        var orientation = GetScreenOrientation();
        if (orientation == screenOrientation)
        {
            return;
        }

        ApplyOrientation(orientation);
    }

    ScreenOrientation GetScreenOrientation()
    {
        if (Screen.height >= Screen.width)
        {
            return ScreenOrientation.Portrait;
        }
        else
        {
            return ScreenOrientation.Landscape;
        }
    }

    void ApplyOrientation(ScreenOrientation orientation)
    {
        screenOrientation = orientation;
        if (orientation == ScreenOrientation.Portrait)
        {
            canvasScaler.matchWidthOrHeight = 0.5f;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0.0f;
        }
        OnOrientationChanged?.Invoke(screenOrientation);
    }
}