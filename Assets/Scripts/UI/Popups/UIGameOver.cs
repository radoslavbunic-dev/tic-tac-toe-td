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
    [SerializeField] protected float winLineDelay = 1;

    bool waitForWinLine = false;

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

        var formatedTime = Utils.ConvertTimeToMilliSeconds(gameOverData.MatchDuration);
        gameDurationText.text = $"{formatedTime}";
        totalMovesText.text = $"{gameOverData.TotalMoves}";

        if (gameOverData.WinningLine != null)
        {
            waitForWinLine = true;
        }
    }

    protected override void Start()
    {
        StartCoroutine(nameof(WaitForWinLine));

    }

    IEnumerator WaitForWinLine()
    {
        if (waitForWinLine)
        {
            yield return new WaitForSeconds(winLineDelay);
        }
        SetListeners();
        Open();
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