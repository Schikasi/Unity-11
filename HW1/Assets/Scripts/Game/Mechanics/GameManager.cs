using System;
using UnityEngine;

namespace Game.Mechanics
{
    [CreateAssetMenu(fileName = "GameManager", menuName = "GameManager", order = 0)]
    public class GameManager : ScriptableObject
    {
        public int Score { get; private set; }
        public int Time { get; private set; }
        public event Action StartGameEvent;
        public event Action PauseGameEvent;
        public event Action ResumeGameEvent;
        public event Action LooseGameEvent;
        public event Action StopGameEvent;
        public event Action<int> UpdateScoreEvent;
        public event Action<int> UpdateTimeEvent;

        public void StartGame()
        {
            Score = 0;
            Time = 0;
            StartGameEvent?.Invoke();
        }

        public void PauseGame()
        {
            PauseGameEvent?.Invoke();
        }

        public void ResumeGame()
        {
            ResumeGameEvent?.Invoke();
        }

        public void LooseGame()
        {
            LooseGameEvent?.Invoke();
        }

        public void StopGame()
        {
            StopGameEvent?.Invoke();
        }

        public void UpdateScore(int val)
        {
            Score = val;
            UpdateScoreEvent?.Invoke(val);
        }

        public void UpdateTime(int val)
        {
            Time = val;
            UpdateTimeEvent?.Invoke(val);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}