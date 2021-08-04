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
        }

        void FixedUpdate()
        {
            float speed = _inputV * _moveSpeed * Time.deltaTime;
            Move(speed);
        }

        private void Rotate(float deltaAngle, float passiveRotation)
        {
            float speedFactor = _rotationCurve.Evaluate(_rotationTime);
            Quaternion rotation = Quaternion.AngleAxis(deltaAngle * speedFactor, Vector3.up) * Quaternion.AngleAxis(passiveRotation, transform.forward);
            transform.rotation = rotation * transform.rotation;
        }

        private void Move(float speed)
        {
            _rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }
    }
}
