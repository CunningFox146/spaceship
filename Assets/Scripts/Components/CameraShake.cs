using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Components
{
    class CameraShake : MonoBehaviour
    {
        private Vector3 _startPos;
        private float _shakeDuration;
        private float _shakeForce;
        
        private void Update()
        {
            if (_shakeDuration == 0f) return;
            
            _shakeDuration = Mathf.Max(_shakeDuration - Time.deltaTime, 0);
            if (_shakeDuration == 0)
            {
                transform.position = _startPos;
                return;
            }

            transform.position = _startPos + Random.insideUnitSphere * _shakeForce;
        }

        public void Shake(float duration, float force)
        {
            _startPos = transform.position;
            _shakeDuration = duration;
            _shakeForce = force;

            Update();
        }
    }
}
