using System;
using UnityEngine;

public class WindowData
{
    public WindowType Type { get; set; }
    public object Data { get; set; }
    public Action<UIWindow> Callback { get; set; }
}