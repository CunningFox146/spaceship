using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float _force;
        [SerializeField] private float _radius;

        void Start()
        {
            var near = Physics.OverlapSphere(transform.position, _radius);
            foreach (Collider coll in near)
            {
                Debug.Log($"coll: {coll.gameObject.name}");
                var rb = coll.transform.root.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(_force, transform.position, _radius);
                }
            }
        }
    }
}
