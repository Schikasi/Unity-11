using UnityEngine;

namespace Game.Mechanics
{
    public class AboutPresenter
    {
        private readonly AboutView _script;
        private readonly GameObject _view;
        private readonly GameControllerMechanics _gcm;

        public AboutPresenter(GameControllerMechanics gcm, GameObject view)
        {
            _gcm = gcm;
            _view = view;
            _view.SetActive(false);
            _script = _view.GetComponent<AboutView>();
        }


        public void Open()
        {
            _view.SetActive(true);
            _script.MainMenuEvent += _gcm.MainMenu;
            _script.MainMenuEvent += Close;
        }

        public void Close()
        {
            _view.SetActive(false);
            _script.MainMenuEvent -= _gcm.MainMenu;
            _script.MainMenuEvent -= Close;
        }
    }
}