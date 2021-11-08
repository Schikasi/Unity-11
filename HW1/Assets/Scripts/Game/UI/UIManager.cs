using System;
using Game.Mechanics;
using Game.UI.Presenter;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameManager gm;
        [SerializeField] private AudioMixer am;

        [Header("UI")] [SerializeField] private GameObject hudView;

        [SerializeField] private GameObject mainMenuView;
        [SerializeField] private GameObject pauseMenuView;
        [SerializeField] private GameObject gameOverView;
        [SerializeField] private GameObject aboutView;
        [SerializeField] private GameObject settingsView;


        private AboutPresenter _about;
        private GameOverPresenter _gameOver;
        private HUDPresenter _hud;
        private MainMenuPresenter _mainMenu;
        private PauseMenuPresenter _pauseMenu;
        private SettingsPresenter _settings;

        private void OnEnable()
        {
            _mainMenu = new MainMenuPresenter(this, mainMenuView);
            _about = new AboutPresenter(this, aboutView);
            _hud = new HUDPresenter(gm, hudView);
            _pauseMenu = new PauseMenuPresenter(gm, pauseMenuView);
            _gameOver = new GameOverPresenter(gm, gameOverView);
            _settings = new SettingsPresenter(this, am, settingsView);
            
            gm.StartGameEvent += ShowHUD;
            gm.ResumeGameEvent += ShowHUD;
            gm.PauseGameEvent += HideHUD;
            gm.LooseGameEvent += HideHUD;

            gm.PauseGameEvent += ShowPauseMenu;
            gm.ResumeGameEvent += HidePauseMenu;
            gm.StopGameEvent += HidePauseMenu;
            gm.LooseGameEvent += HidePauseMenu;

            gm.StopGameEvent += ShowMainMenu;
            gm.LooseGameEvent += ShowGameOver;
            gm.StartGameEvent += HideGameOver;
            gm.StopGameEvent += HideGameOver;


            ShowMainMenu();
        }

        private void HideGameOver()
        {
            gameOverView.SetActive(false);
            _gameOver.Close();
        }

        private void ShowGameOver()
        {
            _gameOver.Open();
            gameOverView.SetActive(true);
        }

        private void HidePauseMenu()
        {
            pauseMenuView.SetActive(false);
            _pauseMenu.Close();
        }

        private void ShowHUD()
        {
            _hud.Open();
            hudView.SetActive(true);
        }

        public void OnPlay()
        {
            gm.StartGame();
        }

        public void ShowAbout(IPresenter presenter = null)
        {
            _about.Open();
            aboutView.SetActive(true);
        }

        public void HideAbout()
        {
            aboutView.SetActive(false);
            _about.Close();
        }

        public void ShowMainMenu()
        {
            _mainMenu.Open();
            mainMenuView.SetActive(true);
        }

        public void HideMainMenu()
        {
            mainMenuView.SetActive(false);
            _mainMenu.Close();
        }

        public void OnExit()
        {
            gm.Exit();
        }

        public void HideHUD()
        {
            hudView.SetActive(false);
            _hud.Close();
        }

        public void ShowPauseMenu()
        {
            _pauseMenu.Open();
            pauseMenuView.SetActive(true);
        }

        public void HideSettings()
        {
            settingsView.SetActive(false);
            _settings.Close();
        }
        
        public void ShowSettings()
        {
            _settings.Open();
            settingsView.SetActive(true);
        }
    }
}