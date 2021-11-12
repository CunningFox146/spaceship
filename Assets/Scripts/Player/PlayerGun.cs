using System.Collections;
using System.Collections.Generic;
using Asteroids.Managers;
using Asteroids.SoundSystem;
using UnityEngine;

namespace Asteroids.Player
{
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private float _fireRate = 0.5f;

        private SoundsEmitter _sound;
        private float _fireCooldown = 0f;

        void Update()
        {
            _sound = GetComponent<SoundsEmitter>();
            _fireCooldown += Time.deltaTime;
        }

        public void Fire(Collider coll)
        {
            if (_fireCooldown < _fireRate)
            {
                return;
            }
            _sound.Play("Player/Shoot");
            _fireCooldown = 0f;
            var bullet = ObjectPooler.Inst.Get(PoolItem.Bullet);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().Launch(gameObject, coll);
        }
    }
}