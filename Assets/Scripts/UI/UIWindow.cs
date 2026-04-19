using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWindow : MonoBehaviour, IUIElement
{
    public static Action<UIWindow> OnClosing;

    public Action<UIWindow> OnWindowClosed;

    [SerializeField] protected GameObject panel;
    public bool IsClosing { get; protected set; }
    public bool IsLoaded { get; protected set; }

    void Awake()
    {
        var canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup)
        {
            canvasGroup.alpha = 0;
        }
        panel.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        GameState.OnStateChanged += OnStateChanged;
    }

    protected virtual void OnDisable()
    {
        GameState.OnStateChanged -= OnStateChanged;
    }

    protected virtual void Start()
    {
        SetListeners();
        Open();
    }

    protected virtual void SetListeners()
    {
        var buttons = GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            btn.onClick.AddListener(() =>
            {
                GameEvents.PlaySFX(AudioClipsId.ButtonClick);
            });
        }
    }

    protected virtual void OnStateChanged(IState fromState, IState toState)
    {
        Close();
    }

    public virtual void Open()
    {
        if (!IsActive())
        {
            panel.SetActive(true);
            GetComponent<Animator>().Play("Open");
        }
        IsClosing = false;
    }

    public virtual void Close()
    {
        if (IsActive())
        {
            IsClosing = true;
            GetComponent<Animator>().Play("Close");
            OnClosing?.Invoke(this);
        }
    }

    public virtual void OnClosed()
    {
        panel.SetActive(false);
        IsClosing = false;
        OnWindowClosed?.Invoke(this);
    }

    public bool IsActive()
    {
        return panel.activeSelf && !IsClosing;
    }
}
