using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewController : MonoBehaviour
{
    public static ViewController Instance { get; private set; }

    public Camera MainCamera { get; private set; }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        RefreshMainCamera();
        SetCamera();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void OnEnable()
    {
        GameState.OnStateChanged += OnStateChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        GameState.OnStateChanged -= OnStateChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshMainCamera();
        SetCamera();
    }

    void OnStateChanged(IState fromState, IState toState)
    {
    }

    void OnPlayerViewChanged()
    {
        SetCamera();
    }

    void RefreshMainCamera()
    {
        MainCamera = Camera.main;
    }

    void SetCamera()
    {
        if (MainCamera == null)
        {
            return;
        }

        if (!MainCamera.orthographic)
        {
            MainCamera.orthographic = true;
        }
    }

    public static Vector2 GetVisibleWorldSize(Camera cam)
    {
        if (cam == null || !cam.orthographic)
        {
            return Vector2.zero;
        }

        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;
        return new Vector2(width, height);
    }

    public static Vector3 GetViewportWorldCenter(Camera cam, float planeZ)
    {
        if (cam == null)
        {
            return Vector3.zero;
        }

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Mathf.Abs(ray.direction.z) < 1e-5f)
        {
            return new Vector3(cam.transform.position.x, cam.transform.position.y, planeZ);
        }

        float t = (planeZ - ray.origin.z) / ray.direction.z;
        return ray.GetPoint(t);
    }
}
