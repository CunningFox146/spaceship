using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class PlayerController : MonoBehaviour, IBoundsTrackable
    {
        [SerializeField] private AnimationCurve _rotationCurve;
        [SerializeField] private float _passiveRotation = 1f;
        [SerializeField] private float _moveSpeed = 90f;
        [SerializeField] private float _rotationSpeed = 180f;
        [SerializeField] private float _maxAngleFactor = 30f;
        [SerializeField] private ParticleSystem _fire;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _model;
        [SerializeField] private GameObject _playerExplosion;

        [SerializeField] private HitOverlay _hitOverlay;
        [SerializeField] private HealthDisplay _healthDisplay;

        private CameraShake _cameraShake;
        private Rigidbody _rb;
        private PlayerGun _gun;
        private Health _health;
        private Coroutine _damageBlink; // Also used to check if we're invincible
        private float _rotationTime;
        private float _inputV;
        private float _inputH;

        public float BoundsOffset => transform.localScale.x;

        void Awake()
        {
            _cameraShake = Camera.main.GetComponent<CameraShake>();
            _rb = GetComponent<Rigidbody>();
            _gun = GetComponent<PlayerGun>();
            _health = GetComponent<Health>();
        }

        void Start()
        {
            BoundsManager.Inst.Track(gameObject);

            _healthDisplay.Init(_health.maxHealth);

            _health.OnHealthChanged += OnHealthChangedHandler;
            _health.OnDeath += OnDeathHandler;
        }

        void Update()
        {
            _inputV = Input.GetAxis("Vertical");
            _inputH = Input.GetAxis("Horizontal");
            
            float angle = _inputH * _rotationSpeed * Time.deltaTime;

            if (_inputH != 0f)
            {
                _rotationTime += Time.deltaTime;
            }
            else
            {
                _rotationTime = 0f;
            }

            Rotate(angle, _passiveRotation * Time.deltaTime);
            UpdateFire();

            if (Input.GetKey(KeyCode.Space))
            {
                _gun.Fire(_collider);
            }
        }

        void FixedUpdate()
        {
            float speed = _inputV * _moveSpeed * Time.deltaTime;
            Move(speed);
        }

        public void OnBoundsReached() => BoundsManager.Inst.TryTeleport(gameObject);

        private void OnDeathHandler()
        {
            if (_damageBlink != null)
            {
                StopCoroutine(_damageBlink);
                _damageBlink = null;
            }
            
            float forceMin = 50f;
            float forceMax = 60f;
            _rb.constraints = RigidbodyConstraints.None;
            _rb.angularVelocity = new Vector3(Random.Range(forceMin, forceMax), Random.Range(forceMin, forceMax), Random.Range(forceMin, forceMax));

            _fire.Play();

            _cameraShake.Shake(1f, .075f);
            Invoke("Explode", 1f);
            
            _hitOverlay.OnHit();
            _healthDisplay.SetHealth(0);

            enabled = false;
        }

        private void OnHealthChangedHandler(int newHealth)
        {
            if (_damageBlink != null)
            {
                StopCoroutine(_damageBlink);
                _damageBlink = null;
            }

            _damageBlink = StartCoroutine(DamageBlink(1f));
            _cameraShake.Shake(.5f, .1f);

            _hitOverlay.OnHit();
            _healthDisplay.SetHealth(newHealth);
        }

        private void Explode()
        {
            _cameraShake.Shake(1f, .1f);
            BoundsManager.Inst.StopTracking(gameObject);
            Instantiate(_playerExplosion).transform.position = transform.position;
            Destroy(gameObject);
        }

        private IEnumerator DamageBlink(float duration)
        {
            float period = 0.1f;

            for (int i = 0; i < duration/ period; i++)
            {
                _model.SetActive(!_model.activeSelf);
                yield return new WaitForSeconds(period);
            }

            _model.SetActive(true);
            _damageBlink = null;
        }

        private void UpdateFire()
        {
            if (_inputV != 0f)
            {
                if (!_fire.isPlaying)
                {
                    _fire.Play();
                }
                return;
            }

            if (_fire.isPlaying)
            {
                _fire.Stop();
            }
        }
        
        private void Rotate(float deltaAngle, float passiveRotation, float velMod = 1f)
        {
            float speedFactor = _rotationCurve.Evaluate(_rotationTime);
            Quaternion rotation = Quaternion.AngleAxis(deltaAngle * speedFactor, Vector3.up);
            if (passiveRotation > 0f)
            {
                rotation *= Quaternion.AngleAxis(passiveRotation, transform.forward);
            }
            transform.rotation = rotation * transform.rotation;
        }

        // 1. Get current velocity and desired velocity.
        // 2. Get angle between them. This angle will be a modifying factor later.
        // 3. Invert desired velocity vector and multiply it by modifying angle factor.
        private void Move(float speed)
        {
            var vel = _rb.velocity;
            var targetVel = transform.forward * speed;

            float angleFactor = Vector3.Angle(vel, targetVel) / _maxAngleFactor; // How much force do we need to fix our direction

            _rb.AddForce(targetVel + (-vel * angleFactor), ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_damageBlink == null)
            {
                _health.DoDelta(-1);
            }
        }
    }
}
