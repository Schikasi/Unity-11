using Game.Mechanics;
using Game.UI.View;
using UnityEngine;

namespace Game.UI.Presenter
{
    public class PauseMenuPresenter
    {
        private readonly GameManager _gm;
        private readonly PauseMenuView _script;

        public PauseMenuPresenter(GameManager gm, GameObject view)
        {
            _gm = gm;
            _script = view.GetComponent<PauseMenuView>();
        }

        public void Open()
        {
            _script.ResumeEvent += _gm.ResumeGame;
            _script.MainMenuEvent += _gm.StopGame;

            _script.SetScore(_gm.Score.ToString());
            _script.SetTime($"{_gm.Time / 60:00}:{_gm.Time % 60:00}");
        }

        public void Close()
        {
            _script.ResumeEvent -= _gm.ResumeGame;
            _script.MainMenuEvent -= _gm.StopGame;
        }
    }
}