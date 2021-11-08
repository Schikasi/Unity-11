using Game.Mechanics;
using Game.UI.View;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.UI.Presenter
{
    public class SettingsPresenter : IPresenter
    {
        private readonly SettingsView _script;
        private readonly UIManager _um;
        private readonly AudioMixer _audioMixer;

        public SettingsPresenter(UIManager um, AudioMixer audioMixer, GameObject view)
        {
            _um = um;
            _audioMixer = audioMixer;
            _script = view.GetComponent<SettingsView>();
        }

        public void Close()
        {
            _script.MainMenuEvent -= OnMainMenu;
            _script.ChangeVolumeEffectsEvent -= SetVolumeEffects;
            float value;
            _audioMixer.GetFloat("EffectsVolume", out value);
            _script.SetVolumeEffect(value > -1.0f);
        }


        public void Open()
        {
            _script.MainMenuEvent += OnMainMenu;
            _script.ChangeVolumeEffectsEvent += SetVolumeEffects;
        }

        private void OnMainMenu()
        {
            _um.HideSettings();
            _um.ShowMainMenu();
        }

        private void SetVolumeEffects(bool value)
        {
            _audioMixer.SetFloat("EffectsVolume", value ? 0f : -80.00f);
        }
    }
}