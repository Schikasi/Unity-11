using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Mechanics
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameManager gm;
        [SerializeField] private Camera mainCamera;

        [SerializeField] private float percentOffset;

        [SerializeField] private float rate;
        private float _timeStartGame;

        public int Score { get; private set; }
        private bool _game;
        private Coroutine _spawnCoroutine;
        private Coroutine _timerCoroutine;
        private readonly HashSet<GameObject> _activeObjects = new HashSet<GameObject>();
        private readonly Stack<GameObject> _poolObjects = new Stack<GameObject>();

        private Pause _pause;

        private void Awake()
        {
            _pause = new Pause(gm);
            gm.StartGameEvent += OnStartGame;
            gm.StopGameEvent += OnStopGame;
            gm.LooseGameEvent += OnStopGame;
        }

        private void OnStopGame()
        {
            _game = false;
            StopCoroutine(_timerCoroutine);
            StopCoroutine(_spawnCoroutine);
            ClearPlayField();
        }

        private void OnStartGame()
        {
            _timeStartGame = Time.time;
            Score = 0;
            _game = true;
            gm.UpdateScore(0);
            gm.UpdateTime(0);

            _spawnCoroutine = StartCoroutine(SpawnBubbles());
            _timerCoroutine = StartCoroutine(GameTimer());
        }

        private IEnumerator SpawnBubbles()
        {
            float t = 0f;
            while (_game)
            {
                t += Time.deltaTime;
                if (t >= rate)
                {
                    SpawnBubble();
                    t = 0f;
                }
                yield return null;
            }
        }

        private IEnumerator GameTimer()
        {
            while (_game)
            {
                gm.UpdateTime((int) (Time.time - _timeStartGame));
                yield return null;
            }
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
            var bm = go.GetComponent<BubbleMechanics>();
            bm.OnClickEvent += ClickBubble;
            bm.BurstEvent += gm.LooseGame;
            return go;
        }


        private void ClickBubble(GameObject go)
        {
            if (Time.timeScale == 0) return;
            go.SetActive(false);
            ++Score;
            gm.UpdateScore(Score);

            _activeObjects.Remove(go);
            _poolObjects.Push(go);
        }
        
        
        private void ClearPlayField()
        {
            foreach (var go in _activeObjects)
            {
                go.SetActive(false);
                _poolObjects.Push(go);
            }
            _activeObjects.Clear();
        }
    }
}