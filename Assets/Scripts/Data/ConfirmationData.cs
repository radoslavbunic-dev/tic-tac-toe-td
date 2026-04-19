using System;
using UnityEngine;

public class ConfirmationData
{
    public Action ConfirmCallback { get; set; }
    public Action DeclineCallback { get; set; }
    public bool ReverseButtons { get; set; }
}