using UnityEngine;

namespace Game.Mechanics
{
    public class MainMenuPresenter : IPresenter
    {
        private readonly UIManager _um;
        private readonly MainMenuView _script;

        public MainMenuPresenter(UIManager um, GameObject view)
        {
            _um = um;
            _script = view.GetComponent<MainMenuView>();
        }

        public void Open()
        {
            _script.PlayEvent += OnPlay;
            _script.AboutEvent += OnAbout;
            _script.ExitEvent += OnExit;
        }

        public void Close()
        {
            _script.PlayEvent -= OnPlay;
            _script.AboutEvent -= OnAbout;
            _script.ExitEvent -= OnExit;
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
            _um.HideMainMenu();
            _um.OnExit();
        }
    }
}