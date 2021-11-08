using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI timeLabel;

        [SerializeField] private Animation animUpdateScore;

        public void OnPause()
        {
            PauseEvent?.Invoke();
        }

        public event Action PauseEvent;

        /// <summary>
        /// Set value without anim.
        /// </summary>
        /// <param name="value">Score value</param>
        public void SetScore(string value)
        {
            scoreLabel.text = value;
        }

        /// <summary>
        /// Set value with anim.
        /// </summary>
        /// <param name="value">Score value</param>
        public void UpdateScore(string value)
        {
            scoreLabel.text = value;
            if (animUpdateScore.isPlaying) animUpdateScore.Rewind();
            animUpdateScore.Play(); 
        }

        public void SetTime(string value)
        {
            timeLabel.text = value;
        }
    }
}