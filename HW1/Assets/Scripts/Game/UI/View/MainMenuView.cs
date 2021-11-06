using System;
using UnityEngine;

namespace Game.UI.View
{
    public class MainMenuView : MonoBehaviour
    {
        public event Action PlayEvent;
        public event Action AboutEvent;
        public event Action ExitEvent;

        public void OnPlay()
        {
            PlayEvent?.Invoke();
        }

        public void OnAbout()
        {
            AboutEvent?.Invoke();
        }

        public void OnExit()
        {
            ExitEvent?.Invoke();
        }
    }
}