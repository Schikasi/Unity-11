using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIScoreMechanics : MonoBehaviour
{
    [SerializeField] private GameObject gameController;
    private Text _st;

    private void Start()
    {
        _st = GetComponent<Text>();
        gameController.GetComponent<GameControllerMechanics>().ScoreUpdateEvent += UpdateScoreHandle;
    }

    private void UpdateScoreHandle(int value)
    {
        _st.text = value.ToString();
    }
}