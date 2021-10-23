using UnityEngine;

namespace Game.Mechanics
{
    public class HUDPresenter
    {
        private readonly HUDView _script;
        private readonly GameObject _view;
        private readonly GameControllerMechanics _gcm;

        public HUDPresenter(GameControllerMechanics gcm, GameObject view)
        {
            _gcm = gcm;
            _view = view;
            _script = _view.GetComponent<HUDView>();
        }

        private void OnScoreChange(int value)
        {
            _script.SetScore($"Score: {value}");
        }

        private void OnTimeChange(int value)
        {
            _script.SetTime($"{(value / 60):00}:{(value % 60):00}");
        }

        public void Open()
        {
            _view.SetActive(true);
            _gcm.ScoreUpdateEvent += OnScoreChange;
            _gcm.TimeUpdateEvent += OnTimeChange;
            _script.PauseEvent += _gcm.Pause;
        }

        public void Close()
        {
            _view.SetActive(false);
            _gcm.ScoreUpdateEvent -= OnScoreChange;
            _gcm.TimeUpdateEvent -= OnTimeChange;
            _script.PauseEvent -= _gcm.Pause;
        }
    }
}