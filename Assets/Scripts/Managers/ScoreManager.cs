using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Util;
using UnityEngine;

namespace Asteroids.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public event Action<int> OnScoreChanged;

        [SerializeField] private int _asteroidScore = 250;
        [SerializeField] private int _shardScore = 100;

        public int highScore = 0;
        private SaveData _data;

        public int Score { get; private set; }
        public bool IsGamePlay { get; private set; } = true;

        public void Start()
        {
            _data = SaveSystem.Load() ?? new SaveData();
            highScore = _data.maxScore;
            Debug.Log($"highScore: {highScore}");
        }

        public void Save()
        {
            if (_data.maxScore < Score)
            {
                _data.maxScore = Score;
            }
            SaveSystem.Save(_data);
        }

        public void DoDelta(int delta)
        {
            Score += delta;
            OnScoreChanged?.Invoke(Score);
        }

        public void SetGameEnd()
        {
            IsGamePlay = false;
        }

        public void TargetKilled(bool isShard)
        {
            DoDelta(isShard ? _shardScore : _asteroidScore);
        }
    }
}