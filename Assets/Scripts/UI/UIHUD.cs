using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHUD : UIWindow
{
    [SerializeField] TextMeshProUGUI matchDurationText;
    [SerializeField] MarkTextKeyPair[] playerNameTexts;
    [SerializeField] MarkTextKeyPair[] playerMoveTexts;
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        TicTacToeManager.OnMatchDurationUpdated += OnMatchDurationUpdated;
        TicTacToeManager.OnMovePlayed += OnMovePlayed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TicTacToeManager.OnMatchDurationUpdated += OnMatchDurationUpdated;
        TicTacToeManager.OnMovePlayed -= OnMovePlayed;
    }

    public void Init(GameData gameData)
    {
        for (int i = 0; i < gameData.Players.Length; i++)
        {
            var player = gameData.Players[i];
            var text = GetValueForMark(playerNameTexts, player.Mark);
            text.text = player.DisplayName;
        }
    }

    protected override void OnStateChanged(IState fromState, IState toState)
    {
        if (toState is GameOverState)
        {
            Close();
        }
    }

    TextMeshProUGUI GetValueForMark(MarkTextKeyPair[] container, TicTacToeMark mark)
    {
        for (int i = 0; i < container.Length; i++)
        {
            if (container[i].Key == mark)
            {
                return container[i].Value;
            }
        }
        Debug.LogError($"unasigned text value for mark {mark} in hud references {container}");
        return null;
    }

    protected override void SetListeners()
    {
        settingsButton.onClick.SetListener(() =>
        {
            GameEvents.OpenSettings();
        });
        exitButton.onClick.SetListener(() =>
        {
            GameEvents.OpenPlayScene();
        });
        base.SetListeners();
    }

    void OnMatchDurationUpdated(float time)
    {
        matchDurationText.text = Utils.ConvertTimeToMilliSeconds(time);
    }

    void OnMovePlayed(TicTacToePlayer player, TicTacToeCell cell)
    {
        var text = GetValueForMark(playerMoveTexts, player.Mark);
        text.text = $"{player.TotalMovesPlayed}";
    }
}
