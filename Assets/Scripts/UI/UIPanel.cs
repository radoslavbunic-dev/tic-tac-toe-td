using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPanel: MonoBehaviour
{
    [SerializeField] protected GameObject panel;

    public virtual void Open()
    {
        if (!IsActive())
        {
            panel.SetActive(true);
        }
    }

    public virtual void Close()
    {
        if (IsActive())
        {
            panel.SetActive(false);
        }
    }

    public bool IsActive()
    {
        return panel.activeSelf;
    }
}
