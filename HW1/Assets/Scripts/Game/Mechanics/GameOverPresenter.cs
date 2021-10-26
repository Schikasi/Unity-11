using UnityEngine;

namespace Game.Mechanics
{
    public class GameOverPresenter
    {
        private readonly GameControllerMechanics _gcm;
        private readonly GameOverView _script;
        private readonly GameObject _view;

        public GameOverPresenter(GameControllerMechanics gcm, GameObject view)
        {
            _gcm = gcm;
            _view = view;
            _view.SetActive(false);
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