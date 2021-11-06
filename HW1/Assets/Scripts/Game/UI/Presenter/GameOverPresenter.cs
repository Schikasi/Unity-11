using Game.Mechanics;
using Game.UI.View;
using UnityEngine;

namespace Game.UI.Presenter
{
    public class GameOverPresenter
    {
        private readonly GameManager _gm;
        private readonly GameOverView _script;

        public GameOverPresenter(GameManager gm, GameObject view)
        {
            _gm = gm;
            _script = view.GetComponent<GameOverView>();
        }


        public void Open()
        {
            _script.RetryEvent += _gm.StartGame;
            _script.MainMenuEvent += _gm.StopGame;
            _script.SetScore($"{_gm.Score}");
            _script.SetTime($"{_gm.Time / 60:00}:{_gm.Time % 60:00}");
        }

        public void Close()
        {
            _script.RetryEvent -= _gm.StartGame;
            _script.MainMenuEvent -= _gm.StopGame;
        }
    }
}