using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _rotationCurve;
        [SerializeField] private float _passiveRotation = 1f;
        [SerializeField] private float _moveSpeed = 90f;
        [SerializeField] private float _rotationSpeed = 180f;
        [Range(0f, 1f)] [SerializeField] private float _angularVelMod = 0f;
        [SerializeField] private ParticleSystem _fire;

        private Rigidbody _rb;
        private float _rotationTime;
        private float _inputV;
        private float _inputH;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
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
        }

        private void UpdateFire()
        {
            if (_inputV != 0f)
            {
                _fire.Play();
            }
            else
            {
                _fire.Stop();
            }
        }

        void FixedUpdate()
        {
            float speed = _inputV * _moveSpeed * Time.deltaTime;
            Move(speed);
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

            if (_angularVelMod < 1f)
            {
                _rb.angularVelocity *= _angularVelMod;

            }
        }

        private void Move(float speed)
        {
            _rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }
    }
}
