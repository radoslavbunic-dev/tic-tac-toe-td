using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatLine: MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI labelText;
    [SerializeField] protected TextMeshProUGUI valueText;

    public void SetLine(string label, string value)
    {
        labelText.text = label;
        valueText.text = value;
    }
}
