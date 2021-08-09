using System.Collections;
using System.Collections.Generic;
using Asteroids;
using UnityEditor;
using UnityEngine;

namespace Asteroids.Managers
{
    public class ObjectPooler : Singleton<ObjectPooler>
    {
        [SerializeField] private List<ObjectPool> _pools;

        private Dictionary<PoolItem, Queue<GameObject>> _objects;

        public override void Awake()
        {
            base.Awake();

            _objects = new Dictionary<PoolItem, Queue<GameObject>>();
            foreach (ObjectPool pool in _pools)
            {
                var queue = new Queue<GameObject>();
                for (int i = 0; i < pool.count; i++)
                {
                    var obj = Instantiate(pool.prefab, transform);
                    obj.SetActive(false);

                    queue.Enqueue(obj);
                }

                _objects.Add(pool.item, queue);
            }
        }

        public GameObject Get(PoolItem item)
        {
            var queue = _objects[item];

            if (queue.Count == 0)
            {
                Debug.LogError($"Pool is empty: {item.ToString()}");
                return null;
            }

            var obj = queue.Dequeue();

            obj.transform.parent = null;
            obj.SetActive(true);

            return obj;
        }

        public GameObject Get(PoolItem item, Vector3 pos)
        {
            var obj = Get(item);

            obj.transform.position = pos;

            return obj;
        }

        public void Return(PoolItem item, GameObject obj)
        {
            obj.transform.parent = transform;
            obj.SetActive(false);
            _objects[item].Enqueue(obj);
        }
    }
}