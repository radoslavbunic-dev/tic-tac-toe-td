using TMPro;
using UnityEngine;

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
        var summary = UserStorage.GetSummary();
        totalGamesTxt.text = summary.TotalGames.ToString();
        winPlayer1Txt.text = summary.Player1Wins.ToString();
        winPlayer2Txt.text = summary.Player2Wins.ToString();
        totalDrawsTxt.text = summary.TotalDraws.ToString();
        averageTimeTxt.text = Utils.ConvertTimeToMilliSeconds(summary.AverageMatchDurationSeconds);
    }

    protected override void SetListeners()
    {
        base.SetListeners();
    }
}