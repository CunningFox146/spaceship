using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField] private List<ObjectPool> _pools;

    private Dictionary<GameObject, Stack<GameObject>> _objects;

    public override void Awake()
    {
        base.Awake();

        _objects = new Dictionary<GameObject, Stack<GameObject>>();
        foreach (ObjectPool pool in _pools)
        {
            var objects = new Stack<GameObject>();

            for (int i = 0; i < pool.count; i++)
            {
                var obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objects.Push(obj);
            }

            _objects.Add(pool.prefab, objects);
        }
    }

    public GameObject Get(GameObject prefab)
    {
        if (_objects.Count == 0)
        {
            Debug.LogError($"Pool is empty! Object: {prefab}");
            return Instantiate(prefab);
        }

        var obj = _objects[prefab].Pop();
        obj.SetActive(true);
        obj.transform.parent = null;

        return obj;
    }

    public void Return(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = transform;
        _objects[prefab].Push(obj);
    }
}
