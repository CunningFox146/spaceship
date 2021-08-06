using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class BulletCollider : MonoBehaviour
    {
        private Bullet _bullet;

        void Awake()
        {
            _bullet = transform.root.GetComponent<Bullet>();
        }

        void OnCollisionEnter(Collision other)
        {
            Debug.Log($"test {other.gameObject.name}");
            //var asteroid = other.transform.root.GetComponent<Asteroid>();
            //if (asteroid != null)
            //{
            //    asteroid.OnHit(other);
            //    ObjectPooler.Inst.Return(transform.root.gameObject);
            //}
        }
    }
}
