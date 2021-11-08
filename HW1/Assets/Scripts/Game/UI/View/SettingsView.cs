using System;
using UnityEngine;

namespace Game.UI.View
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private GameObject checkMark;
        public event Action MainMenuEvent;
        public event Action<bool> ChangeVolumeEffectsEvent;
        // Start is called before the first frame update

        public void ToggleCheckMark()
        {
            checkMark.SetActive(!checkMark.activeSelf);
            ChangeVolumeEffectsEvent?.Invoke(checkMark.activeSelf);
        }

        public void SetCheckMark(bool value)
        {
            checkMark.SetActive(value);
        }
    
        public void OnMainMenu()
        {
            MainMenuEvent?.Invoke();
        }
    }
}
