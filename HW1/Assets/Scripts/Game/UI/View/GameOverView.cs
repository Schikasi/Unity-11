using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Mechanics
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Text gameOverLabel;
        [SerializeField] private Text scoreLabel;

        [SerializeField] private Button retryButton;
        [SerializeField] private Button mainMenuButton;

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
            scoreLabel.text = value;
        }
    }
}