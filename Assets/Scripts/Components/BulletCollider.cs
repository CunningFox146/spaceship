using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class BulletCollider : MonoBehaviour
    {
        void OnCollisionEnter(Collision other)
        {
            Debug.Log($"OnCollisionEnter{other}");
            var asteroid = other.gameObject.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.OnHit(other);
                ObjectPooler.Inst.Return(transform.root.gameObject);
            }
        }
    }
}
