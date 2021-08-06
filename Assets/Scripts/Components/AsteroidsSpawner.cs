//#define DEBUG_POSITIONS

using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.ScriptableObjects;
using Scripts.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Components
{
    public class AsteroidsSpawner : MonoBehaviour
    {
        [SerializeField] private AsteroidWave[] _waves;
        [SerializeField] private GameObject _asteroid;
        [SerializeField] private GameObject _asteroidShard;
        [SerializeField] private float _directionAngle;
        [SerializeField] private Vector2 _speed;
        [SerializeField] private float _spawnOffset = 2f;

        private float AsteroidSpeed
        {
            get => RandomUtil.WithVariance(_speed.x, _speed.y);
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
#if !DEBUG_POSITIONS
            ReleaseWave();
#endif
        }

#if DEBUG_POSITIONS
        void Update()
        {
            var bounds = BoundsManager.Inst;
            float xMax = bounds.BoundsWidth;
            float zMax = bounds.BoundsHeight;

            Debug.DrawLine(new Vector3(Random.Range(-xMax, xMax), 0f, zMax + _spawnOffset),
                new Vector3(xMax, 0f, zMax + _spawnOffset), Color.red);
            Debug.DrawLine(new Vector3(xMax + _spawnOffset, 0f, Random.Range(-zMax, zMax)),
                new Vector3(xMax + _spawnOffset, 0f, zMax), Color.green);
            Debug.DrawLine(new Vector3(Random.Range(-xMax, xMax), 0f, -zMax - _spawnOffset),
                new Vector3(xMax, 0f, -zMax - _spawnOffset), Color.blue);
            Debug.DrawLine(new Vector3(-xMax - _spawnOffset, 0f, Random.Range(-zMax, zMax)),
                new Vector3(-xMax - _spawnOffset, 0f, zMax), Color.cyan);
        }
#endif

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
            var bounds = BoundsManager.Inst;
            float xMax = bounds.BoundsWidth;
            float zMax= bounds.BoundsHeight;

            return Random.Range(0, 3) switch
            {
                0 => new Vector3(Random.Range(-xMax, xMax), 0f, zMax + _spawnOffset),
                1 => new Vector3(xMax + _spawnOffset, 0f, Random.Range(-zMax, zMax)),
                2 => new Vector3(Random.Range(-xMax, xMax), 0f, -zMax - _spawnOffset),
                _ => new Vector3(-xMax - _spawnOffset, 0f, Random.Range(-zMax, zMax)),
            };
        }
    }
}
