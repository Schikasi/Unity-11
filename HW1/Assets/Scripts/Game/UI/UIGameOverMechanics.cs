using UnityEngine;

public class UIGameOverMechanics : MonoBehaviour
{
    [SerializeField] private GameObject gameController;
    [SerializeField] private GameObject view;

    private void Awake()
    {
        gameController.GetComponent<GameControllerMechanics>().StateGameChangedEvent += ShowScreen;
    }

    private void ShowScreen(bool state)
    {
        view.SetActive(!state);
    }
}