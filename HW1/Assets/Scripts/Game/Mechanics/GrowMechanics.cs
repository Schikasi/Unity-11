using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GrowMechanics : MonoBehaviour
{
    public delegate void BurstHandler();

    public delegate void OnClickHandler(GameObject gameObject);

    public event BurstHandler BurstEvent;
    public event OnClickHandler OnClickEvent;

    [SerializeField] private float speedGrowUp = 1;

    [SerializeField] private Vector2 startScale = new Vector2(0.1f, 0.1f);

    [SerializeField] private Vector2 endScale = new Vector2(1f, 1f);


    private float _growPercent = 0.0f;
    private SpriteRenderer _sr;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        transform.localScale = startScale;
        _growPercent = 0.0f;
        _sr.color = Random.ColorHSV(0f, 1f, 0.7f, 0.85f, 0.7f, 1f, 0.65f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.Equals(endScale))
        {
            BurstEvent?.Invoke();
            gameObject.SetActive(false);
        }

        _growPercent += Time.deltaTime * speedGrowUp;
        transform.localScale = Vector2.Lerp(startScale, endScale, _growPercent);
    }

    private void OnMouseDown()
    {
        OnClickEvent?.Invoke(gameObject);
        gameObject.SetActive(false);
    }
}