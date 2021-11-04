using UnityEngine;

namespace Game.Mechanics
{
    public class HUDPresenter
    {
        private readonly GameManager _gm;
        private readonly HUDView _script;

        public HUDPresenter(GameManager gm, GameObject view)
        {
            _gm = gm;
            _script = view.GetComponent<HUDView>();
        }

        public void Open()
        {
            _gm.UpdateScoreEvent += OnScoreChange;
            _gm.UpdateTimeEvent += OnTimeChange;
            _script.PauseEvent += _gm.PauseGame;
        }

        public void Close()
        {
            _gm.UpdateScoreEvent -= OnScoreChange;
            _gm.UpdateTimeEvent -= OnTimeChange;
            _script.PauseEvent -= _gm.PauseGame;
        }


        private void OnScoreChange(int value)
        {
            _script.SetScore($"{value}");
        }

        private void OnTimeChange(int value)
        {
            _script.SetTime($"{value / 60:00}:{value % 60:00}");
        }
    }
}