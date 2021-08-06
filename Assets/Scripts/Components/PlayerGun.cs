using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private float _fireRate = 0.5f;

        private float _fireCooldown = 0f;

        void Update()
        {
            _fireCooldown += Time.deltaTime;
        }

        public void Fire(Collider coll)
        {
            if (_fireCooldown < _fireRate)
            {
                return;
            }

            _fireCooldown = 0f;
            var bullet = ObjectPooler.Inst.Get(PoolItem.Bullet);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().Launch(gameObject, coll);
        }
    }
}