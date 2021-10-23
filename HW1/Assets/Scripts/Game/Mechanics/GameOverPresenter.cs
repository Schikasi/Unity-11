using UnityEngine;

namespace Game.Mechanics
{
    public class GameOverPresenter
    {
        private readonly GameOverView _script;
        private readonly GameObject _view;
        private readonly GameControllerMechanics _gcm;

        public GameOverPresenter(GameControllerMechanics gcm, GameObject view)
        {
            _gcm = gcm;
            _view = view;
            _script = _view.GetComponent<GameOverView>();
        }


        public void Open()
        {
            _view.SetActive(true);
            _script.RetryEvent += _gcm.Play;
            _script.MainMenuEvent += _gcm.MainMenu;
            _script.SetScore($"Score: {_gcm.Score}");
            _script.RetryEvent += Close;
            _script.MainMenuEvent += Close;
        }

        public void Close()
        {
            _view.SetActive(false);
            _script.RetryEvent -= _gcm.Play;
            _script.MainMenuEvent -= _gcm.MainMenu;
            _script.RetryEvent -= Close;
            _script.MainMenuEvent -= Close;
        }
    }
}