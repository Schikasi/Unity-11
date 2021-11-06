using Game.Mechanics;
using Game.UI.View;
using UnityEngine;

namespace Game.UI.Presenter
{
    public class AboutPresenter : IPresenter
    {
        private readonly AboutView _script;
        private readonly UIManager _um;

        public AboutPresenter(UIManager um, GameObject view)
        {
            _um = um;
            _script = view.GetComponent<AboutView>();
        }

        public void Close()
        {
            _script.MainMenuEvent -= OnMainMenu;
        }


        public void Open()
        {
            _script.MainMenuEvent += OnMainMenu;
        }

        private void OnMainMenu()
        {
            _um.HideAbout();
            _um.ShowMainMenu();
        }
    }
}