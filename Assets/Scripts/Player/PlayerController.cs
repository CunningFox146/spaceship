using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Managers;
using Asteroids.SoundSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Player
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
        
        private SoundsEmitter _sound;
        private Rigidbody _rb;
        private PlayerGun _gun;
        private Health _health;
        private Coroutine _blinkCoroutine; // Also used to check if we're invincible
        private AudioSource _fireSound;
        private bool _isCutscene;
        private float _rotationTime;
        private float _inputV;
        private float _inputH;
        
        void Awake()
        {
            _sound = GetComponent<SoundsEmitter>();
            _rb = GetComponent<Rigidbody>();
            _gun = GetComponent<PlayerGun>();
            _health = GetComponent<Health>();
        }

        void Start()
        {
            _fireSound = _sound.Play("Player/ShipLoop");

            BoundsManager.Inst.Add(gameObject);
            
            _health.OnHealthChanged += OnHealthChangedHandler;
            _health.OnDeath += OnDeathHandler;

            StartCutscene();
            UpdateFire();
        }

        private void StartCutscene()
        {
            _sound.Play("Player/ShipStart");

            var originalLayer = _model.layer;
            var targetLayer = LayerMask.NameToLayer("Default");
            var startPos = _model.transform.localPosition;

            _isCutscene = true;
            _model.layer = targetLayer;
            _fire.gameObject.layer = targetLayer;
            _model.transform.localPosition += Vector3.forward * -3.5f;
            _fire.Play();

            LeanTween.moveLocal(_model, startPos, 1f)
                .setEaseOutCubic()
                .setOnComplete(() =>
                {
                    _isCutscene = false;
                    _model.layer = originalLayer;
                    _fire.gameObject.layer = originalLayer;
                    _fire.Stop();
                });
        }

        void Update()
        {
            if (!_isCutscene)
            {
                _inputV = Input.GetAxis("Vertical");
                _inputH = Input.GetAxis("Horizontal");
            }
            
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

        private void OnCollisionEnter(Collision collision)
        {
            if (_blinkCoroutine == null) // We're invincible
            {
                _health.DoDelta(-1);
            }
        }

        public void UpdateBounds()
        {
            var pos = transform.position;
            float offset = transform.localScale.x;

            if (!BoundsManager.GetInBounds(pos, offset))
            {
                transform.position = BoundsManager.GetNewPos(transform.position, transform.localScale.x);
            }
        }

        private void OnDeathHandler()
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
                _blinkCoroutine = null;
            }
            
            float forceMin = 50f;
            float forceMax = 60f;
            _rb.constraints = RigidbodyConstraints.None;
            _rb.angularVelocity = new Vector3(Random.Range(forceMin, forceMax), Random.Range(forceMin, forceMax), Random.Range(forceMin, forceMax));

            _fire.Play();
            _sound.Play("Player/PlayerDeathPre");

            CameraManager.Inst.Shake(1f, .075f);
            Invoke("Explode", 1f);

            ScoreManager.Inst.Save();
            
            enabled = false;
        }

        private void OnHealthChangedHandler(int newHealth)
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
                _blinkCoroutine = null;
            }

            _blinkCoroutine = StartCoroutine(DamageBlinkCoroutine(1f));
            CameraManager.Inst.Shake(.5f, .1f);
            _sound.Play("Player/PlayerHit");
        }

        private void Explode()
        {
            _sound.Play("Player/PlayerDeath");
            CameraManager.Inst.Shake(1f, .1f);
            BoundsManager.Inst.Remove(gameObject);
            Instantiate(_playerExplosion).transform.position = transform.position;
            _collider.gameObject.SetActive(false);
            _model.gameObject.SetActive(false);
            enabled = false;
        }

        private IEnumerator DamageBlinkCoroutine(float duration)
        {
            float period = 0.1f;

            for (int i = 0; i < duration/ period; i++)
            {
                _model.SetActive(!_model.activeSelf);
                yield return new WaitForSeconds(period);
            }

            _model.SetActive(true);
            _blinkCoroutine = null;
        }

        private void UpdateFire()
        {
            if (_inputV != 0f || _isCutscene)
            {
                if (!_fire.isPlaying)
                {
                    _fireSound.Play();
                    _fire.Play();
                }

                return;
            }

            if (_fire.isPlaying)
            {
                _fireSound.Pause();
                _fire.Stop();
            }
        }
        
        private void Rotate(float deltaAngle, float passiveRotation)
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
    }
}
