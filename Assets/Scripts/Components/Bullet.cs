using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class Bullet : MonoBehaviour, IBoundsTrackable
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _duration = 3f;

        private Rigidbody _rb;

        public float BoundsOffset => transform.localScale.x;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void OnBoundsReached() => Destroy(gameObject);

        public void Launch(GameObject launcher, Collider coll)
        {
            //Physics.IgnoreCollision(_collider, coll);
            _rb.velocity = _speed * launcher.transform.forward;

            transform.rotation = Quaternion.Euler(launcher.transform.forward);
        }

    }
}