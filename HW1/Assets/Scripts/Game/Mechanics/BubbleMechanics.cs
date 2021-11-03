using System.Security.Cryptography;
using UnityEngine;

namespace Game.Mechanics
{
    public class BubbleMechanics : MonoBehaviour
    {
        public delegate void BurstHandler();

        public delegate void OnClickHandler(GameObject gameObject);

        [Range(0.1f, 1f)] [Tooltip("Grow up percent per second")] [SerializeField]
        private float speedGrowUp = 1;

        [SerializeField] private Vector2 startScale = new Vector2(0.1f, 0.1f);

        [SerializeField] private Vector2 endScale = new Vector2(1f, 1f);

        [SerializeField] private GameObject particlePrefab;

        private float _growPercent;
        private SpriteRenderer _sr;

        private void Start()
        {
            transform.localScale = startScale;
        }

        private void Update()
        {
            if (transform.localScale.Equals(endScale))
            {
                BurstEvent?.Invoke();
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
            _sr.color = Random.ColorHSV(0f, 1f, 0.75f, 0.80f, 0.75f, 1f, 0.65f, 0.7f);
        }

        private void OnDisable()
        {
            transform.localScale = startScale;
            _growPercent = 0.0f;
        }

        private void OnMouseDown()
        {
            SpawnParticles();
            OnClickEvent?.Invoke(gameObject);
        }

        private void SpawnParticles()
        {
            var go = Instantiate(particlePrefab, transform.localPosition, Quaternion.identity);
            var particles = go.GetComponent<ParticleSystem>();
            var main = particles.main;
            var shape = particles.shape;
            shape.radius = transform.localScale.x / 8;
            main.startColor = _sr.color;
            particles.Play();
            Destroy(particles.gameObject, main.duration);
        }

        public event BurstHandler BurstEvent;
        public event OnClickHandler OnClickEvent;
    }
}