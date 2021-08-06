using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class Asteroid : MonoBehaviour, IBoundsTrackable
    {
        [SerializeField] private int _shardCount = 3;
        [SerializeField] private bool _isShard = false;
        [SerializeField] private GameObject _explosion;
        [SerializeField] private GameObject _shard;

        private Rigidbody _rb;
        private Coroutine _layerCoroutine;

        public float BoundsOffset => transform.localScale.x;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            //Launch(Vector3.right, 2f);
        }

        public void OnBoundsReached() => BoundsManager.Inst.TryTeleport(gameObject);
        
        public void Launch(float speed)
        {
            _rb.velocity = transform.forward * speed;

            var targetLayer = LayerMask.NameToLayer("Default");
            gameObject.layer = targetLayer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = targetLayer;
            }
            
            StartCoroutine(UpdateLayer());

            //var perpendicular = -Vector2.Perpendicular(new Vector2(direction.x, direction.z)); // To set rotation to desired location we need an inverted perpendicular
            //_rb.angularVelocity = new Vector3(perpendicular.x, 0, perpendicular.y) * (speed * 0.75f);
        }

        private IEnumerator UpdateLayer()
        {
            var pos = transform.position;
            var bounds = BoundsManager.Inst;

            while (bounds.ShouldTeleport(gameObject, BoundsOffset))
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f); // To make sure it's not on the edge, bc it looks ugly

            var targetLayer = LayerMask.NameToLayer("Bounds");
            gameObject.layer = targetLayer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = targetLayer;
            }
            
            BoundsManager.Inst.Track(gameObject);

            if (_layerCoroutine != null)
            {
                StopCoroutine(_layerCoroutine);
                _layerCoroutine = null;
            }
        }

        public void OnAttacked(Collision collision)
        {
            BoundsManager.Inst.StopTracking(gameObject);
            ObjectPooler.Inst.Return(_isShard ? PoolItem.MeteorShard : PoolItem.Meteor, gameObject);

            if (!_isShard)
            {
                float range = 0.5f;
                for (int i = 1; i <= _shardCount; i++)
                {
                    float percent = (float)i / (float)_shardCount;
                    var offset = new Vector3(Mathf.Cos(percent * Mathf.PI * 2) * range, 0f,
                        Mathf.Sin(percent * Mathf.PI * 2) * range);

                    var shard = ObjectPooler.Inst.Get(PoolItem.MeteorShard, transform.position + offset);
                    shard.transform.LookAt(-transform.position);
                    shard.GetComponent<Asteroid>().Launch(2f);
                }
            }

            var fx = Instantiate(_explosion);
            fx.transform.position = collision.contacts[0].point;
        }
    }
}