using UnityEngine;

namespace Game.Mechanics
{
    public class Pause
    {
        public Pause(GameManager gm)
        {
            gm.PauseGameEvent += OnPause;
            gm.ResumeGameEvent += OnResume;
            gm.StartGameEvent += OnResume;
        }

        private void OnResume()
        {
            Time.timeScale = 1;
        }

        private void OnPause()
        {
            Time.timeScale = 0;
        }
    }
}