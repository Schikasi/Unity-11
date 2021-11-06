using Game.Mechanics;
using Game.UI.View;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Game.UI.Presenter
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
            _script.SetScore($"{_gm.Score}");
            _script.SetTime($"{_gm.Time / 60:00}:{_gm.Time % 60:00}");
            
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
            _script.UpdateScore($"{value}");
        }

        private void OnTimeChange(int value)
        {
            _script.SetTime($"{value / 60:00}:{value % 60:00}");
        }
    }
}