using System.Collections;
using System.Collections.Generic;
using log4net.Core;
using UnityEngine;

public class MainMechanics : MonoBehaviour
{
    public delegate void ScoreUpdateHandler(int value);

    public delegate void GameOverHandler();

    public event ScoreUpdateHandler ScoreUpdateEvent;
    public event GameOverHandler GameOverEvent;

    [SerializeField] private GameObject prefab;

    [SerializeField] private Camera _camera;

    [SerializeField] private float rate;

    [SerializeField] private float percentOffset;

    [SerializeField] private Vector2 maxSize;

    private float _curTime = 0f;
    private int score = 0;
    private bool game = true;
    private Stack<GameObject> poolObjects = new Stack<GameObject>();
    private Stack<GameObject> activeObjects = new Stack<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!game) return;
        _curTime = Mathf.Clamp(_curTime + Time.deltaTime, 0, rate);
        if (_curTime.Equals(rate))
        {
            _curTime = 0.0f;
            SpawnBubble();
        }
    }

    private void SpawnBubble()
    {
        GameObject bubble;
        if (poolObjects.Count == 0)
        {
            bubble = CreateBubble();
        }
        else
        {
            bubble = poolObjects.Pop();
        }

        var position = _camera.ScreenToWorldPoint(
            new Vector2(
                Random.Range(Screen.width * percentOffset, Screen.width * (1 - percentOffset)),
                Random.Range(Screen.height * percentOffset, Screen.height * (1 - percentOffset))
            )
        );
        position.z = 0.0f;

        bubble.transform.SetPositionAndRotation(position, Quaternion.identity);
        bubble.SetActive(true);
        activeObjects.Push(bubble);
    }

    private GameObject CreateBubble()
    {
        var go = Instantiate(prefab);
        var instGM = go.GetComponent<GrowMechanics>();
        instGM.OnClickEvent += AddScore;
        instGM.BurstEvent += EndGame;
        return go;
    }

    private void EndGame()
    {
        game = false;
        while (activeObjects.Count !=0)
        {
            activeObjects.Pop().SetActive(false);
        }
        GameOverEvent?.Invoke();
    }

    private void AddScore(GameObject gameObject)
    {
        ++score;
        poolObjects.Push(gameObject);
        ScoreUpdateEvent?.Invoke(score);
    }
}