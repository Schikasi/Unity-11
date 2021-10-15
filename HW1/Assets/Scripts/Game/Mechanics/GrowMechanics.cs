using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GrowMechanics : MonoBehaviour
{
    [SerializeField] private float speedGrowUp = 1;

    [SerializeField] private Vector2 startScale = new Vector2(0.1f,0.1f);

    [SerializeField] private Vector2 endScale = new Vector2(1f,1f);

    private float _growPercent = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        transform.localScale = startScale;
        _growPercent = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.Equals(endScale) ) gameObject.SetActive(false);
        _growPercent += Time.deltaTime * speedGrowUp;
        transform.localScale =Vector2.Lerp(startScale, endScale, _growPercent);
    }

    private void OnMouseDown()
    {
        gameObject.SetActive(false);
    }
}
