using System.Collections;
using System.Collections.Generic;
using Asteroids.Managers;
using UnityEngine;

namespace Asteroids.Player
{
    public class Bullet : MonoBehaviour, IBoundsTrackable
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private Collider _collider;

        private Rigidbody _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void UpdateBounds()
        {
            if (!BoundsManager.GetInBounds(transform.position, transform.localScale.x))
            {
                BoundsManager.Inst.Remove(gameObject);
                ObjectPooler.Inst.Return(PoolItem.Bullet, gameObject);
            }
        }

        public void Launch(GameObject launcher, Collider coll)
        {
            //Physics.IgnoreCollision(_collider, coll);
            BoundsManager.Inst.Add(gameObject);
            _rb.velocity = _speed * launcher.transform.forward;

            transform.rotation = launcher.transform.rotation;
        }

        public void OnHit()
        {
            BoundsManager.Inst.Remove(gameObject);
            ObjectPooler.Inst.Return(PoolItem.Bullet, gameObject);
        }
    }
}