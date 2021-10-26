using UnityEngine;

namespace Game.Mechanics
{
    public class MainMenuPresenter
    {
        private readonly GameControllerMechanics _gcm;
        private readonly MainMenuView _script;
        private readonly GameObject _view;

        public MainMenuPresenter(GameControllerMechanics gcm, GameObject view)
        {
            _gcm = gcm;
            _view = view;
            _view.SetActive(false);
            _script = _view.GetComponent<MainMenuView>();
        }

        public void Open()
        {
            _view.SetActive(true);
            _script.PlayEvent += _gcm.Play;
            _script.CreditsEvent += _gcm.Credits;
            _script.ExitEvent += _gcm.Exit;
            _script.PlayEvent += Close;
        }

        public void Close()
        {
            _view.SetActive(false);
            _script.PlayEvent -= _gcm.Play;
            _script.CreditsEvent -= _gcm.Credits;
            _script.ExitEvent -= _gcm.Exit;
        }
    }
}