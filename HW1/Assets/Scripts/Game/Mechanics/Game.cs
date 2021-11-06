using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameManager gm;
        [SerializeField] private Camera mainCamera;

        [SerializeField] private float percentOffset;

        [SerializeField] private float rate;
        private readonly HashSet<GameObject> _activeObjects = new HashSet<GameObject>();
        private readonly Stack<GameObject> _poolObjects = new Stack<GameObject>();
        private bool _game;

        private Pause _pause;
        private Coroutine _spawnCoroutine;
        private Coroutine _timerCoroutine;
        private float _timeStartGame;

        private void Awake()
        {
            _pause = new Pause(gm);
            gm.StartGameEvent += OnStartGame;
            gm.StopGameEvent += OnStopGame;
            gm.LooseGameEvent += OnLooseGame;
        }

        private void OnLooseGame()
        {
            _game = false;
            StopCoroutine(_timerCoroutine);
            StopCoroutine(_spawnCoroutine);
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
            ClearPlayField();
            _timeStartGame = Time.time;
            _game = true;
            _spawnCoroutine = StartCoroutine(SpawnBubbles());
            _timerCoroutine = StartCoroutine(GameTimer());
        }

        private IEnumerator SpawnBubbles()
        {
            var t = 0f;
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
            bubble.SetActive(true);
            var bm = bubble.GetComponent<BubbleMechanics>();
            bm.Spawn(position);
            gm.LooseGameEvent += bm.Froze;
            _activeObjects.Add(bubble);
        }

        private GameObject CreateBubble()
        {
            var go = Instantiate(prefab);
            var bm = go.GetComponent<BubbleMechanics>();
            bm.OnClickEvent += ClickBubble;
            bm.EndScaleEvent += gm.LooseGame;
            return go;
        }


        private void ClickBubble(GameObject go)
        {
            gm.UpdateScore(gm.Score+1);

            DeactivateBubble(go);
            _activeObjects.Remove(go);
            _poolObjects.Push(go);
        }


        private void ClearPlayField()
        {
            foreach (var go in _activeObjects)
            {
                DeactivateBubble(go);
                _poolObjects.Push(go);
            }

            _activeObjects.Clear();
        }

        private void DeactivateBubble(GameObject go)
        {
            go.SetActive(false);
            var bm = go.GetComponent<BubbleMechanics>();
            gm.LooseGameEvent -= bm.Froze;
            bm.Reset();
        }


        private void AddToPool(GameObject go)
        {
            if (go == null) return;
            _poolObjects.Push(go);
        }
    }
}