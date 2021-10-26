using System;
using UnityEngine;

namespace Game.Mechanics
{
    public class MainMenuView : MonoBehaviour
    {
        public event Action PlayEvent;
        public event Action CreditsEvent;
        public event Action ExitEvent;

        public void OnPlay()
        {
            PlayEvent?.Invoke();
        }

        public void OnCredit()
        {
            CreditsEvent?.Invoke();
        }

        public void OnExit()
        {
            ExitEvent?.Invoke();
        }
    }
}