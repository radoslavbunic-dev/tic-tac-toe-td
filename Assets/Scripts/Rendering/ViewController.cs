using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewController : MonoBehaviour
{
    public static event Action<ScreenOrientation> OnOrientationChanged;

    public Camera MainCamera { get; private set; }
    ScreenOrientation screenOrientation;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        RefreshMainCamera();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshMainCamera();
    }

    void RefreshMainCamera()
    {
        MainCamera = Camera.main;
    }

    void Update()
    {
        var orientation = GetScreenOrientation();
        if (orientation == screenOrientation)
        {
            return;
        }

        screenOrientation = orientation;
        OnOrientationChanged?.Invoke(screenOrientation);
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
}
