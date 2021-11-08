using System;
using UnityEngine;

namespace Game.UI.Widget
{
    public class CheckBox : MonoBehaviour
    {

        [SerializeField] private GameObject checkMark;
        public event Action<bool> CheckBoxToggleEvent;
    
    
        public void ToggleCheckMark()
        {
            checkMark.SetActive(!checkMark.activeSelf);
            CheckBoxToggleEvent?.Invoke(checkMark.activeSelf);
        }
    
        public void SetCheckMark(bool value)
        {
            checkMark.SetActive(value);
        }
    
    }
}
