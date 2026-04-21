using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIWindows : MonoBehaviour
{
    public static Action<WindowData> ShowWindow;

    [SerializeField] RectTransform content;
    Dictionary<UIWindow, AsyncOperationHandle<GameObject>> loadedAssets = new Dictionary<UIWindow, AsyncOperationHandle<GameObject>>();

    void OnEnable()
    {
        ShowWindow += OnShowWindow;
    }

    void OnDisable()
    {
        ShowWindow -= OnShowWindow;
    }

    protected virtual void OnShowWindow(WindowData data)
    {
        foreach (var item in loadedAssets)
        {
            if (item.Key is IPersistentWindow persistentWindow && persistentWindow.Type == data.Type)
            {
                data.Callback?.Invoke(item.Key);
                return;
            }
        }
        Addressables.LoadAssetAsync<GameObject>(data.Type.ToString()).Completed += operation => OnWindowAssetReady(operation, data);
    }

    void OnWindowAssetReady(AsyncOperationHandle<GameObject> operation, WindowData data)
    {
        if (operation.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Failed to load window " + data.Type);
            new LoadingPlaySceneState().Enter(null);
            return;
        }

        var window = Instantiate(operation.Result, content).GetComponent<UIWindow>();
        window.name = data.Type.ToString();
        window.OnWindowClosed += OnWindowClosed;
        if (!loadedAssets.ContainsKey(window))
        {
            loadedAssets.Add(window, operation);
        }
        data.Callback?.Invoke(window);
    }

    void OnWindowClosed(UIWindow window)
    {
        if (window)
        {
            window.OnWindowClosed -= OnWindowClosed;
            if (loadedAssets.TryGetValue(window, out var asyncOperation))
            {
                Addressables.Release(asyncOperation);
                loadedAssets.Remove(window);
            }
            Destroy(window.gameObject);
        }
    }
}