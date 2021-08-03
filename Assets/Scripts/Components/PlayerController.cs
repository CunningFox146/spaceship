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

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            float inputH = Input.GetAxis("Horizontal");
            float inputV = Input.GetAxis("Vertical");
            
            float speed = inputV * _moveSpeed * Time.deltaTime;
            float angle = inputH * _rotationSpeed * Time.deltaTime;

            if (inputH != 0f)
            {
                _rotationTime += Time.deltaTime;
            }
            else
            {
                _rotationTime = 0f;
            }

            Rotate(angle, _passiveRotation * Time.deltaTime);
            Move(speed);
        }

        private void Rotate(float deltaAngle, float passiveRotation)
        {
            float speedFactor = _rotationCurve.Evaluate(_rotationTime);
            Debug.Log(speedFactor);
            Quaternion rotation = Quaternion.AngleAxis(deltaAngle * speedFactor, Vector3.forward) * Quaternion.AngleAxis(passiveRotation, transform.up);
            transform.rotation = rotation * transform.rotation;
        }

        private void Move(float speed)
        {
            _rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);
        }
    }
}
