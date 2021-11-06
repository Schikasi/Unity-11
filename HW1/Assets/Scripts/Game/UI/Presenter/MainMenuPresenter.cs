using Game.Mechanics;
using Game.UI.View;
using UnityEngine;

namespace Game.UI.Presenter
{
    public class MainMenuPresenter : IPresenter
    {
        private readonly MainMenuView _script;
        private readonly UIManager _um;

        public MainMenuPresenter(UIManager um, GameObject view)
        {
            _um = um;
            _script = view.GetComponent<MainMenuView>();
        }

        public void Close()
        {
            _script.PlayEvent -= OnPlay;
            _script.AboutEvent -= OnAbout;
            _script.ExitEvent -= OnExit;
        }

        public void Open()
        {
            _script.PlayEvent += OnPlay;
            _script.AboutEvent += OnAbout;
            _script.ExitEvent += OnExit;
        }

        private void OnPlay()
        {
            _um.HideMainMenu();
            _um.OnPlay();
        }

        private void OnAbout()
        {
            _um.HideMainMenu();
            _um.ShowAbout();
        }

        private void OnExit()
        {
            //_um.HideMainMenu();
            _um.OnExit();
        }
    }
}