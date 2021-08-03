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

            float rotation = inputH * _moveSpeed * Time.deltaTime;
            float speed = inputV * _moveSpeed * Time.deltaTime;
            float angle = rotation * _rotationSpeed * Time.deltaTime;

            _rotationTime = inputH != 0f ? _rotationTime + Time.deltaTime : 0f;

            Rotate(angle, _passiveRotation * Time.deltaTime);
            Move(speed);
        }

        private void Rotate(float deltaAngle, float passiveRotation)
        {
            float speedFactor = _rotationCurve.Evaluate(_rotationTime);
            Debug.Log(speedFactor);
            Quaternion rotation = Quaternion.AngleAxis(deltaAngle * speedFactor, Vector3.up); /* *
                                  Quaternion.AngleAxis(passiveRotation, Vector3.forward);*/
            transform.rotation *= rotation;
        }

        private void Move(float speed)
        {
            _rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);
        }
    }
}
