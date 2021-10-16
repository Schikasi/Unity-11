using System.Collections.Generic;
using UnityEngine;

public class GameControllerMechanics : MonoBehaviour
{
    public delegate void ScoreUpdateHandler(int value);

    public delegate void StateGameChangedHandler(bool state);

    [SerializeField] private GameObject prefab;

    [SerializeField] private Camera _camera;

    [SerializeField] private float percentOffset;

    [SerializeField] private float rate;

    //[SerializeField] private Vector2 maxSize;

    private float _curTime;
    private readonly HashSet<GameObject> _activeObjects = new HashSet<GameObject>();
    private bool _game = true;
    private readonly Stack<GameObject> _poolObjects = new Stack<GameObject>();
    private int _score;


    // Start is called before the first frame update
    private void Start()
    {
        StateGameChangedEvent?.Invoke(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_game)
        {
            if (Input.GetKeyDown(KeyCode.Space)) RestartGame();
        }
        else
        {
            _curTime = Mathf.Clamp(_curTime + Time.deltaTime, 0, rate);
            if (_curTime.Equals(rate))
            {
                _curTime = 0.0f;
                SpawnBubble();
            }
        }
    }

    public event ScoreUpdateHandler ScoreUpdateEvent;
    public event StateGameChangedHandler StateGameChangedEvent;

    private void RestartGame()
    {
        _curTime = 0.0f;
        _score = 0;
        ScoreUpdateEvent?.Invoke(_score);
        _game = true;
        StateGameChangedEvent?.Invoke(_game);
    }

    private void SpawnBubble()
    {
        GameObject bubble;
        if (_poolObjects.Count == 0)
            bubble = CreateBubble();
        else
            bubble = _poolObjects.Pop();

        var position = _camera.ScreenToWorldPoint(
            new Vector2(
                Random.Range(Screen.width * percentOffset, Screen.width * (1 - percentOffset)),
                Random.Range(Screen.height * percentOffset, Screen.height * (1 - percentOffset))
            )
        );
        position.z = 0.0f;

        bubble.transform.SetPositionAndRotation(position, Quaternion.identity);
        bubble.SetActive(true);

        _activeObjects.Add(bubble);
    }

    private GameObject CreateBubble()
    {
        var go = Instantiate(prefab);
        var gm = go.GetComponent<BubbleMechanics>();
        gm.OnClickEvent += AddScore;
        gm.BurstEvent += EndGame;
        return go;
    }

    private void EndGame()
    {
        _game = false;

        foreach (var go in _activeObjects)
        {
            go.SetActive(false);
            _poolObjects.Push(go);
        }

        _activeObjects.Clear();

        StateGameChangedEvent?.Invoke(_game);
    }

    private void AddScore(GameObject go)
    {
        ++_score;
        _activeObjects.Remove(go);
        _poolObjects.Push(go);
        ScoreUpdateEvent?.Invoke(_score);
    }
}