using System.Collections;
using System.Collections.Generic;
using Asteroids.SoundSystem;
using UnityEngine;

namespace Asteroids.Asteroid
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float _force;
        [SerializeField] private float _radius;

        void Start()
        {
            var sound = GetComponent<SoundsEmitter>();
            if (sound != null)
            {
                sound.Play("MeteorDown");
            }
            
            var near = Physics.OverlapSphere(transform.position, _radius);
            foreach (Collider coll in near)
            {
                var rb = coll.transform.root.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(_force, transform.position, _radius);
                }
            }
        }
    }
}
