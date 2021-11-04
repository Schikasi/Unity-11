using System;
using TMPro;
using UnityEngine;

namespace Game.UI.View
{
    public class PauseMenuView : MonoBehaviour
    {
        public event Action ResumeEvent;
        public event Action MainMenuEvent;

        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI time;

        public void OnResume()
        {
            ResumeEvent?.Invoke();
        }

        public void OnMainMenu()
        {
            MainMenuEvent?.Invoke();
        }

        public void SetScore(string val)
        {
            score.text = val;
        }

        public void SetTime(string val)
        {
            time.text = val;
        }
    }
}