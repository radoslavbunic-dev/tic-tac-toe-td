using System;
using UnityEngine;

public class PopupData
{
    public IState SourceState { get; set; }
    public PopupType Type { get; set; } = PopupType.None;
    public Action<UIPopup> Callback { get; set; }
    public bool IsPersistent { get; set; } = false;
    public bool SkipCleanUp { get; set; } = false;
    public bool AllowMultiple { get; set; } = false;
    public float Duration { get; set; } = 3;
    public string Title { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}