using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMechanics : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField] private float rate;

    [SerializeField] private float percentOffset;

    private float _curTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _curTime = Mathf.Clamp(_curTime + Time.deltaTime, 0, rate);
        if (_curTime.Equals(rate))
        {
            _curTime = 0.0f;
            var position = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(Screen.width * percentOffset,
                    Screen.width * (1 - 2 * percentOffset)),
                Random.Range(Screen.height * percentOffset, Screen.height * (1 - 2 * percentOffset))));
            position.z = 0.0f;
            Instantiate(prefab,position,Quaternion.identity);
        }
    }
}