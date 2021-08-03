using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _rotationSpeed = 1f;

        private Rigidbody _rb;
        private Quaternion _rotation;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            float rotation = Input.GetAxis("Horizontal") * _moveSpeed * Time.deltaTime;
            float speed = Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;
            float angle = rotation * _rotationSpeed * Time.deltaTime;

            Rotate(angle);
            Move(speed);
        }

        private void Rotate(float deltaAngle)
        {
            transform.rotation *= Quaternion.AngleAxis(deltaAngle, Vector3.up);
        }

        private void Move(float speed)
        {
            _rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);
        }
    }
}
