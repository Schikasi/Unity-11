using UnityEngine;

namespace Game.Mechanics
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
            _script.SetScore($"Score: {_gm.Score}");
        }

        public void Close()
        {
            _script.RetryEvent -= _gm.StartGame;
            _script.MainMenuEvent -= _gm.StopGame;
        }
    }
}