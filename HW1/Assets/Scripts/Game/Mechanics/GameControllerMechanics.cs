using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameControllerMechanics : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private Camera _camera;

    [SerializeField] private float percentOffset;

    [SerializeField] private float rate;

    [SerializeField] private GameObject hudView;
    [SerializeField] private GameObject mainMenuView;
    [SerializeField] private GameObject pauseMenuView;
    [SerializeField] private GameObject gameOverView;
    private readonly HashSet<GameObject> _activeObjects = new HashSet<GameObject>();
    private readonly Stack<GameObject> _poolObjects = new Stack<GameObject>();
    private bool _game = true;
    private GameOverPresenter _gameOver;
    private HUDPresenter _hud;
    private MainMenuPresenter _mainMenu;
    private PauseMenuPresenter _pauseMenu;

    private Coroutine _spawnCoroutine;
    private Coroutine _timerCoroutine;

    //[SerializeField] private Vector2 maxSize;

    private float _timeStartGame;

    public int Score { get; private set; }

    private void Awake()
    {
        //Input.backButtonLeavesApp = true;
    }


    // Start is called before the first frame update
    private void Start()
    {
        _hud = new HUDPresenter(this, hudView);
        _mainMenu = new MainMenuPresenter(this, mainMenuView);
        _pauseMenu = new PauseMenuPresenter(this, pauseMenuView);
        _gameOver = new GameOverPresenter(this, gameOverView);
        _mainMenu.Open();
    }

    public event Action<int> TimeUpdateEvent;

    public event Action<int> ScoreUpdateEvent;
    public event Action<bool> StateGameChangedEvent;

    private void SpawnBubble()
    {
        var bubble = _poolObjects.Count == 0 ? CreateBubble() : _poolObjects.Pop();

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
        gm.OnClickEvent += ClickBubble;
        gm.BurstEvent += EndGame;
        return go;
    }

    private void EndGame()
    {
        _game = false;
        StateGameChangedEvent?.Invoke(_game);

        ClearPlayField();
        _gameOver.Open();
    }

    private void ClearPlayField()
    {
        foreach (var go in _activeObjects)
        {
            go.SetActive(false);
            _poolObjects.Push(go);
        }

        StopCoroutine(_timerCoroutine);
        StopCoroutine(_spawnCoroutine);
        _activeObjects.Clear();
        _hud.Close();
    }

    private void ClickBubble(GameObject go)
    {
        if(Time.timeScale == 0) return;
        go.SetActive(false);
        ++Score;
        ScoreUpdateEvent?.Invoke(Score);

        _activeObjects.Remove(go);
        _poolObjects.Push(go);
    }

    private IEnumerator Spawn()
    {
        while (_game)
        {
            SpawnBubble();
            yield return new WaitForSeconds(rate);
        }
    }

    private IEnumerator GameTimer()
    {
        while (_game)
        {
            TimeUpdateEvent?.Invoke((int) (Time.time - _timeStartGame));
            yield return new WaitForSeconds(1);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;

        _pauseMenu.Open();
    }

    public void Play()
    {
        Time.timeScale = 1;
        _timeStartGame = Time.time;
        Score = 0;
        _game = true;
        StateGameChangedEvent?.Invoke(_game);
        _hud.Open();
        ScoreUpdateEvent?.Invoke(Score);
        TimeUpdateEvent?.Invoke(0);
        _spawnCoroutine = StartCoroutine(Spawn());
        _timerCoroutine = StartCoroutine(GameTimer());

    }


    public void Credits()
    {
        Debug.Log("Разработчик: Schikasi\n" +
                  "Художник: RinnFox");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pauseMenu.Close();
    }

    public void MainMenu()
    {
        if (_game)
        {
            ClearPlayField();
            _game = false;
        }

        _mainMenu.Open();
    }
}