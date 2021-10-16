using UnityEngine;

public class BubbleMechanics : MonoBehaviour
{
    public delegate void BurstHandler();

    public delegate void OnClickHandler(GameObject gameObject);

    [Range(0.1f,1f)]
    [Tooltip("Grow up percent per second")]
    [SerializeField] private float speedGrowUp = 1;

    [SerializeField] private Vector2 startScale = new Vector2(0.1f, 0.1f);

    [SerializeField] private Vector2 endScale = new Vector2(1f, 1f);


    private float _growPercent;
    private SpriteRenderer _sr;

    // Start is called before the first frame update
    private void Start()
    {
        transform.localScale = startScale;
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.localScale.Equals(endScale))
        {
            BurstEvent?.Invoke();
            gameObject.SetActive(false);
        }
        else
        {
            _growPercent += Time.deltaTime * speedGrowUp;
            transform.localScale = Vector2.Lerp(startScale, endScale, _growPercent);
        }
    }

    private void OnEnable()
    {
        _sr ??= GetComponent<SpriteRenderer>();
        _sr.color = Random.ColorHSV(0f, 1f, 0.7f, 0.85f, 0.7f, 1f, 0.65f, 0.8f);
    }

    private void OnDisable()
    {
        transform.localScale = startScale;
        _growPercent = 0.0f;
    }

    private void OnMouseDown()
    {
        OnClickEvent?.Invoke(gameObject);
        gameObject.SetActive(false);
    }

    public event BurstHandler BurstEvent;
    public event OnClickHandler OnClickEvent;
}