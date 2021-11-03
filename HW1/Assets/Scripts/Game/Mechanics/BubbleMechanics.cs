using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Mechanics
{
    public class BubbleMechanics : MonoBehaviour
    {
        [Range(0.1f, 1f)] [Tooltip("Grow up percent per second")] [SerializeField]
        private float speedGrowUp = 1;

        [SerializeField] private Vector2 startScale = new Vector2(0.1f, 0.1f);
        [SerializeField] private Vector2 endScale = new Vector2(1f, 1f);
        [SerializeField] private GameObject particlePrefab;
        [SerializeField] private GameObject viewBubble;

        private float _growPercent;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private bool _isActive = false;

        private void OnEnable()
        {
            _sr = viewBubble.GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            Reset();
        }

        private IEnumerator GrowUp()
        {
            while (_isActive)
            {
                if (transform.localScale.Equals(endScale))
                {
                    EndScaleEvent?.Invoke();
                    yield break;
                }
                else
                {
                    _growPercent += Time.deltaTime * speedGrowUp;
                    transform.localScale = Vector2.Lerp(startScale, endScale, _growPercent);
                }
                yield return null;
            }
        }

        private void OnMouseDown()
        {
            if (!_isActive || Time.timeScale == 0) return;
            SpawnParticles();
            OnClickEvent?.Invoke(gameObject);
        }

        public void Spawn(Vector2 position)
        {
            transform.localPosition = position;
            _isActive = true;
            StartCoroutine(GrowUp());
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

        public void Froze()
        {
            _isActive = false;
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        public void Reset()
        {
            _sr.color = Random.ColorHSV(0f, 1f, 0.75f, 0.80f, 0.75f, 1f, 0.65f, 0.7f);
            transform.localScale = startScale;
            _growPercent = 0.0f;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.velocity = Vector2.zero;
        }

        public event Action EndScaleEvent;
        public event Action<GameObject> OnClickEvent;
    }
}