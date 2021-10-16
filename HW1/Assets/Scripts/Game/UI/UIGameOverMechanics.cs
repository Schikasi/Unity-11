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
}