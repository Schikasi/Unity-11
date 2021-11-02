using UnityEngine;

namespace Game.Mechanics
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
            _script.ResumeEvent += Close;
            _script.MainMenuEvent += Close;
        }

        public void Close()
        {
            _script.ResumeEvent -= _gm.ResumeGame;
            _script.MainMenuEvent -= _gm.StopGame;
            _script.ResumeEvent -= Close;
            _script.MainMenuEvent -= Close;
        }
    }
}