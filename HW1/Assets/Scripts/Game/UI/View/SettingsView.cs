using System;
using Game.UI.Widget;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI.View
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private CheckBox volumeEffect;
        public event Action MainMenuEvent;
        public event Action<bool> ChangeVolumeEffectsEvent;

        private void OnEnable()
        {
            volumeEffect.CheckBoxToggleEvent += ChangeVolumeEffect;
        }

        private void OnDisable()
        {
            volumeEffect.CheckBoxToggleEvent -= ChangeVolumeEffect;
        }

        private void ChangeVolumeEffect(bool value)
        {
            ChangeVolumeEffectsEvent?.Invoke(value);
        }

        public void SetVolumeEffect(bool value)
        {
            volumeEffect.SetCheckMark(value);
        }
    
        public void OnMainMenu()
        {
            MainMenuEvent?.Invoke();
        }
    }
}
