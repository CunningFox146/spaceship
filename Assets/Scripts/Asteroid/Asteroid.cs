using System.Collections;
using System.Collections.Generic;
using Asteroids.Managers;
using Asteroids.Player;
using Asteroids.Util;
using UnityEngine;

namespace Asteroids.Asteroid
{
    public class Asteroid : MonoBehaviour, IBoundsTrackable
    {
        [SerializeField] private bool _isShard = false;
        [SerializeField] private Vector2 _shardsCount;
        [SerializeField] private GameObject _explosion;
        [SerializeField] private GameObject _shard;

        private Rigidbody _rb;
        
        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void OnCollisionEnter(Collision collision)
        {
            var bullet = collision.transform.root.GetComponent<Bullet>();
            if (bullet)
            {
                bullet.OnHit();
                OnAttacked(collision);
            }
        }

        public void UpdateBounds()
        {
            var pos = transform.position;
            float offset = transform.localScale.x;

            if (!BoundsManager.GetInBounds(pos, offset))
            {
                transform.position = BoundsManager.GetNewPos(transform.position, transform.localScale.x);
            }
        }
        
        // Gets called only from spawner
        public void Launch(float speed)
        {
            _rb.velocity = transform.forward * speed;

            var targetLayer = LayerMask.NameToLayer("Default");
            gameObject.layer = targetLayer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = targetLayer;
            }
            
            StartCoroutine(UpdateLayerCoroutine());
        }

        private IEnumerator UpdateLayerCoroutine()
        {
            float start = Time.time; // If there's too many asteroids they SOMETIMES never reach the bounds
            while (!BoundsManager.GetInBounds(transform.position, transform.localScale.x) && Time.time - start < 5f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f); // To make sure it's not on the edge, bc it looks ugly

            BoundsManager.Inst.Add(gameObject);

            var targetLayer = LayerMask.NameToLayer("Bounds");
            gameObject.layer = targetLayer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = targetLayer;
            }
        }

        public void OnAttacked(Collision collision)
        {
            BoundsManager.Inst.Remove(gameObject);
            ObjectPooler.Inst.Return(_isShard ? PoolItem.MeteorShard : PoolItem.Meteor, gameObject);

            ScoreManager.Inst.TargetKilled(_isShard);

            if (!_isShard)
            {
                SpawnShards();
            }

            var fx = Instantiate(_explosion);
            fx.transform.position = collision.contacts[0].point;
        }

        private void SpawnShards()
        {
            float range = 0.5f;
            int count = (int)(_shardsCount.x + Random.Range(0f, 1f) * _shardsCount.y + 0.5f);
            float angleOffset = Mathf.PI * 2 * Random.Range(0f, 1f);
            for (int i = 1; i <= count; i++)
            {
                float percent = (float)i / (float)count;
                float angle = angleOffset + percent * Mathf.PI * 2;
                var offset = new Vector3(Mathf.Cos(angle) * range, 0f, Mathf.Sin(angle) * range);

                var shard = ObjectPooler.Inst.Get(PoolItem.MeteorShard, transform.position + offset);
                shard.GetComponent<Rigidbody>().velocity = offset.normalized * Random.Range(3f, 5f);
                BoundsManager.Inst.Add(shard);
            }
        }
    }
}