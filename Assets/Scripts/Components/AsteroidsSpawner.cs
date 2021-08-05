using System.Collections;
using System.Collections.Generic;
using Scripts.ScriptableObjects;
using Scripts.Util;
using UnityEngine;

namespace Scripts.Components
{
    public class AsteroidsSpawner : MonoBehaviour
    {
        [SerializeField] private AsteroidWave[] _waves;
        [SerializeField] private GameObject _asteroid;
        [SerializeField] private GameObject _asteroidShard;
        [SerializeField] private float _directionAngle;
        [SerializeField] private Vector2 _speed;
        [SerializeField] private float _spawnOffset = 1f;

        private float AsteroidSpeed
        {
            get => RandomUtil.RandomWithVariance(_speed.x, _speed.y);
            set { }
        }

        private List<GameObject> _asteriods;
        private int _wave = 0;

        void Awake()
        {
            _asteriods = new List<GameObject>();
        }

        void Start()
        {
            Invoke("ReleaseWave", 1f);
        }

        private void ReleaseWave()
        {
            if (_waves.Length - 1 < _wave)
            {
                return;
            }

            var pooler = ObjectPooler.Inst;
            var data = _waves[_wave];

            for (int i = 0; i < data.asteroidsCount; i++)
            {
                var pos = GetSpawnPosition();

                var asteroid = pooler.Get(_asteroid);
                _asteriods.Add(asteroid);
                Debug.Log($"Spawned at {pos}");
                asteroid.transform.position = pos;
                var cmp = asteroid.GetComponent<Asteroid>();
                cmp.Launch(AsteroidSpeed);
            }

            _wave++;
        }

        private Vector3 GetSpawnPosition()
        {
            return Vector3.down;
            //var bounds = BoundsManager.Inst;
            //float maxX = bounds.maxPos.x;
            //float maxZ = bounds.maxPos.z;
            //float minX = bounds.minPos.x;
            //float minZ = bounds.minPos.z;

            //return Random.Range(0, 3) switch
            //{
            //    0 => new Vector3(minX - _spawnOffset, 0f, Random.Range(minZ, maxZ)),
            //    1 => new Vector3(Random.Range(minX, maxX), 0f, minZ - _spawnOffset),
            //    2 => new Vector3(maxX + _spawnOffset, 0f, Random.Range(minZ, maxZ)),
            //    _ => new Vector3(Random.Range(minX, maxX), 0f, maxZ - _spawnOffset),
            //};
        }
        
    }
}
