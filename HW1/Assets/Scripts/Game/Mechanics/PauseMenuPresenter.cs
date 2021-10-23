using UnityEngine;

namespace Game.Mechanics
{
    public class PauseMenuPresenter
    {
        private readonly PauseMenuView _script;
        private readonly GameObject _view;
        private readonly GameControllerMechanics _gcm;

        public PauseMenuPresenter(GameControllerMechanics gcm, GameObject view)
        {
            _gcm = gcm;
            _view = view;
            _script = _view.GetComponent<PauseMenuView>();
        }

        public void Open()
        {
            _view.SetActive(true);
            _script.ResumeEvent += _gcm.ResumeGame;
            _script.MainMenuEvent += _gcm.MainMenu;
            _script.ResumeEvent += Close;
            _script.MainMenuEvent += Close;
        }

        public void Close()
        {
            _view.SetActive(false);
            _script.ResumeEvent -= _gcm.ResumeGame;
            _script.MainMenuEvent -= _gcm.MainMenu;
            _script.ResumeEvent -= Close;
            _script.MainMenuEvent -= Close;
        }
    }
}