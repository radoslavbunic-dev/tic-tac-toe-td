using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIBackground : MonoBehaviour, IUIElement
{
    [SerializeField] GameObject panel;
    public Action<UIBackground> OnBackgroundClosed;

    void Awake()
    {
        var canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup)
        {
            canvasGroup.alpha = 0;
        }
    }

    public virtual void Open()
    {
        GetComponent<Animator>().Play("Open");
    }

    public virtual void Close()
    {
        GetComponent<Animator>().Play("Close");
    }

    public virtual void OnClosed()
    {
        OnBackgroundClosed?.Invoke(this);
    }
}
