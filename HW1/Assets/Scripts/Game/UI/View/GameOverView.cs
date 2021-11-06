using System;
using TMPro;
using UnityEngine;

namespace Game.UI.View
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI time;

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
            score.text = value;
        }

        public void SetTime(string val)
        {
            time.text = val;
        }
    }
}