using UnityEngine;

namespace Game.Mechanics
{
    public class AboutPresenter: IPresenter
    {
        private readonly UIManager _um;
        private readonly AboutView _script;

        public AboutPresenter(UIManager um, GameObject view)
        {
            _um = um;
            _script = view.GetComponent<AboutView>();
        }


        public void Open()
        {
            _script.MainMenuEvent += OnMainMenu;
        }

        public void Close()
        {
            _script.MainMenuEvent -= OnMainMenu;
        }

        private void OnMainMenu()
        {
            _um.HideAbout();
            _um.ShowMainMenu();
        }

    }
}