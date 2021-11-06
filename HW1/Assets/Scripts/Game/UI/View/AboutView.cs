using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View
{
    public class AboutView : MonoBehaviour
    {
        [SerializeField] private Text gameNameLabel;

        [SerializeField] private Text creditsLabel;

        [SerializeField] private Button mainMenuButton;

        public event Action MainMenuEvent;

        public void OnMainMenu()
        {
            MainMenuEvent?.Invoke();
        }
    }
}