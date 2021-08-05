using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private bool _isShard;
        [SerializeField] private GameObject _collider;

        private Rigidbody _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            BoundsManager.Inst.Track(gameObject);

            //Launch(Vector3.right, 2f);
        }
        
        public void Launch(float speed)
        {
            transform.LookAt(Vector3.zero);
            _rb.velocity = transform.forward * speed;
            //var perpendicular = -Vector2.Perpendicular(new Vector2(direction.x, direction.z)); // To set rotation to desired location we need an inverted perpendicular
            //_rb.angularVelocity = new Vector3(perpendicular.x, 0, perpendicular.y) * (speed * 0.75f);
        }
    }
}