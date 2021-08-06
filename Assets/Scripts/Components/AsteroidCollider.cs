using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class AsteroidCollider : MonoBehaviour
    {
        [SerializeField] private Asteroid _asteroid;
        
        void OnCollisionEnter(Collision collision)
        {
            var bullet = collision.transform.root.GetComponent<Bullet>();
            if (bullet)
            {
                bullet.OnHit();
                _asteroid.OnAttacked(collision);
                return;
            }

            // TODO: Player hit logic
        }
    }
}