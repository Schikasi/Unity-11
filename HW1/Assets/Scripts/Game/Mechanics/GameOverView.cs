using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private Text GameOverLabel;

    [SerializeField] private Text ScoreLabel;

    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button RetryButton;

    public event Action MainMenuEvent;
    public event Action RetryEvent;

    public void OnMainMenu()
    {
        MainMenuEvent?.Invoke();
    }

    public void OnRetry()
    {
        RetryEvent?.Invoke();
    }

    public void SetScore(string value)
    {
        ScoreLabel.text = value;
    }
}