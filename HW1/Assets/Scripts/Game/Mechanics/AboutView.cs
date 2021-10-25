using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutView : MonoBehaviour
{
    [SerializeField] private Text GameNameLabel;

    [SerializeField] private Text CreditsLabel;

    [SerializeField] private Button MainMenuButton;

    public event Action MainMenuEvent;

    public void OnMainMenu()
    {
        MainMenuEvent?.Invoke();
    }
}