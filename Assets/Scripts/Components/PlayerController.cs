using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 10f;

        private Rigidbody _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            float speedX = Input.GetAxis("Horizontal") * _moveSpeed * Time.deltaTime;
            float speedY = Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;
            
            _rb.velocity = new Vector3(speedX, speedY);
        }
    }
}
