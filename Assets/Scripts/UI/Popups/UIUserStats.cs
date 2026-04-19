using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

using UnityEngine.SocialPlatforms;

public class UIUserStats : UIPopup
{
    [SerializeField] protected TextMeshProUGUI totalGamesTxt;
    [SerializeField] protected TextMeshProUGUI winPlayer1Txt;
    [SerializeField] protected TextMeshProUGUI winPlayer2Txt;
    [SerializeField] protected TextMeshProUGUI totalDrawsTxt;
    [SerializeField] protected TextMeshProUGUI averageTimeTxt;

    public override void Init(PopupData data)
    {
        base.Init(data);
        totalGamesTxt.text = "0";
        totalGamesTxt.text = "0";
        totalGamesTxt.text = "0";
        totalGamesTxt.text = "0";
        totalGamesTxt.text = "0";
    }

    protected override void SetListeners()
    {
        base.SetListeners();
    }
}