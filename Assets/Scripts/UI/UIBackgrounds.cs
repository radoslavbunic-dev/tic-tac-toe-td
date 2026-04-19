using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIBackgrounds : MonoBehaviour
{
    [SerializeField] RectTransform content;
    [SerializeField] CanvasGroup darkOverlay;
    Dictionary<UIBackground, AsyncOperationHandle<GameObject>> loadedAssets = new Dictionary<UIBackground, AsyncOperationHandle<GameObject>>();

    Dictionary<int, HashSet<Type>> openStates = new Dictionary<int, HashSet<Type>>()
    {
        {
            1,
            new HashSet<Type>
            {
                typeof(MainMenuState),
            }
        },
    };

    protected virtual void OnEnable()
    {
        GameState.OnStateChanged += OnStateChanged;
    }

    protected virtual void OnDisable()
    {
        GameState.OnStateChanged -= OnStateChanged;
    }

    void Awake()
    {
        darkOverlay.alpha = 1;
        StartCoroutine(RemoveDark(0.2f));
    }

    IEnumerator RemoveDark(float duration)
    {
        yield return new WaitForSeconds(0.1f);
        float max = duration;
        while (duration > 0)
        {
            darkOverlay.alpha = duration / max;
            yield return duration -= Time.deltaTime;
        }
        Destroy(darkOverlay.gameObject);
    }

    void OnStateChanged(IState fromState, IState toState)
    {
        foreach (var stateSet in openStates)
        {
            if (stateSet.Value.Contains(toState.GetType()))
            {
                ChangeBackground(stateSet.Key);
                return;
            }
        }
        CloseBackground();
    }

    void ChangeBackground(int index)
    {
        var assetKey = $"UIBackground{index}";
        Addressables.LoadAssetAsync<GameObject>(assetKey).Completed += operation => OnWindowAssetReady(operation);
    }

    void CloseBackground()
    {
        var backgrounds = content.GetComponentsInChildren<UIBackground>();
        foreach (var item in backgrounds)
        {
            item.OnBackgroundClosed += OnBackgroundClosed;
            item.Close();
        }
    }

    void OnWindowAssetReady(AsyncOperationHandle<GameObject> operation)
    {
        CloseBackground();
        if (operation.Status != AsyncOperationStatus.Succeeded)
        {
            return;
        }

        var bkg = Instantiate(operation.Result, content).GetComponent<UIBackground>();
        bkg.Open();
        if (!loadedAssets.ContainsKey(bkg))
        {
            loadedAssets.Add(bkg, operation);
        }
    }

    void OnBackgroundClosed(UIBackground bkg)
    {
        if (bkg)
        {
            bkg.OnBackgroundClosed -= OnBackgroundClosed;
            if (loadedAssets.TryGetValue(bkg, out var operation))
            {
                loadedAssets.Remove(bkg);
                Addressables.Release(operation);
            }
            Destroy(bkg.gameObject);
        }
    }
}
