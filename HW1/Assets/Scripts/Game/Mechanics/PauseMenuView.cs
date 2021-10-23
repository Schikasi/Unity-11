using System;
using UnityEngine;

public class PauseMenuView : MonoBehaviour
{
    public event Action ResumeEvent;
    public event Action MainMenuEvent;

    public void OnResume()
    {
        ResumeEvent?.Invoke();
    }

    public void OnMainMenu()
    {
        MainMenuEvent?.Invoke();
    }
}