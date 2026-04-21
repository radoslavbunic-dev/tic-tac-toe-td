using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameOver : UIWindow
{
    public Action OnPlayAgainButtonClicked;
    public Action OnCloseButtonClicked;

    [SerializeField] protected Button playAgainButton;
    [SerializeField] protected Button closeButton;
    [SerializeField] protected TextMeshProUGUI gameStatusText;
    [SerializeField] protected TextMeshProUGUI gameDurationText;
    [SerializeField] protected TextMeshProUGUI totalMovesText;

    public void Init(GameOverData gameOverData)
    {
        if (gameOverData.Winner != null)
        {
            gameStatusText.text = $"{gameOverData.Winner.DisplayName} WON";
        }
        else
        {
            gameStatusText.text = $"{gameOverData.GameStatus}";
        }

        var formatedTime = Utils.ConvertTimeToSeconds(gameOverData.MatchDuration);
        gameDurationText.text = $"{formatedTime}";
        totalMovesText.text = $"{gameOverData.TotalMoves}";
    }

    protected override void SetListeners()
    {
        playAgainButton.onClick.SetListener(() =>
        {
            OnPlayAgainButtonClicked?.Invoke();
            Close();
        });
        closeButton.onClick.SetListener(() =>
        {
            OnCloseButtonClicked?.Invoke();
            Close();
        });

        base.SetListeners();
    }
}