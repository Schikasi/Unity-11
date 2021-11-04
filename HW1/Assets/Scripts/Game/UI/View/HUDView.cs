using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Mechanics
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI timeLabel;

        [SerializeField] private Button pauseButton;

        public void OnPause()
        {
            PauseEvent?.Invoke();
        }

        public event Action PauseEvent;

        public void SetScore(string value)
        {
            scoreLabel.text = value;
        }

        public void SetTime(string value)
        {
            timeLabel.text = value;
        }
    }
}