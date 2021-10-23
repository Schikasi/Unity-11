using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Mechanics
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private Text ScoreLabel;

        [SerializeField] private Text TimeLabel;

        [SerializeField] private Button PauseButton;

        public void OnPause()
        {
            PauseEvent?.Invoke();
        }

        public event Action PauseEvent;

        public void SetScore(string value)
        {
            ScoreLabel.text = value;
        }

        public void SetTime(string value)
        {
            TimeLabel.text = value;
        }
    }
}