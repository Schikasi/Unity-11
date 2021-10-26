using System.Collections.Generic;
using UnityEngine;

public class GameControllerMechanics : MonoBehaviour
{
    public delegate void ScoreUpdateHandler(int value);

    public delegate void StateGameChangedHandler(bool state);

    [SerializeField] private GameObject prefab;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float percentOffset;

    [SerializeField] private float rate;
    private readonly HashSet<GameObject> _activeObjects = new HashSet<GameObject>();
    private readonly Stack<GameObject> _poolObjects = new Stack<GameObject>();

    private float _curTime;
    private bool _game = true;
    private int _score;

    private void Start()
    {
        StateGameChangedEvent?.Invoke(_game);
    }

    private void Update()
    {
        if (!_game)
        {
            if (Input.GetKeyDown(KeyCode.Space)) RestartGame();
        }
        else
        {
            _curTime += Time.deltaTime;
            if (_curTime >= rate)
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
        var bubble = _poolObjects.Count == 0 ? CreateBubble() : _poolObjects.Pop();

        var position = mainCamera.ScreenToWorldPoint(
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
        if (!_game) return;
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
        if (!go.activeSelf) return;
        go.SetActive(false);
        ++_score;
        _activeObjects.Remove(go);
        _poolObjects.Push(go);
        ScoreUpdateEvent?.Invoke(_score);
    }
}