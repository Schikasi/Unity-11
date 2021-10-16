using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOverMechanics : MonoBehaviour
{
    [SerializeField] private GameObject gameController;

    private void Awake()
    {
        gameController.GetComponent<GameControllerMechanics>().StateGameChangedEvent += ShowScreen;
    }

    private void ShowScreen(bool state)
    {
        gameObject.SetActive(!state);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
