using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        [SerializeField] private int _asteroidScore = 250;
        [SerializeField] private int _shardScore = 100;

        public event Action<int> OnScoreChanged;

        public int Score { get; private set; }
        public bool IsGamePlay { get; private set; } = true;

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