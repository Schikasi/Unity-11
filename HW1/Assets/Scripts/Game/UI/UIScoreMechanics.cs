using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIScoreMechanics : MonoBehaviour
{
    private Text _st;
    [SerializeField] private GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController.GetComponent<GameControllerMechanics>().ScoreUpdateEvent += UpdateScoreHandle;
    }

    private void OnValidate()
    {
        _st = GetComponent<Text>();
    }
    
    void UpdateScoreHandle(int value)
    {
        _st.text = value.ToString();
    }
}