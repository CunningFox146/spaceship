using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public event Action<int> OnScoreChanged;
        public event Action<bool> OnGameEnd;

        public int Score { get; private set; }
        public bool IsWin { get; private set; } = false;
        public bool IsGamePlay { get; private set; } = true;

        public void DoDelta(int delta)
        {
            Score += delta;
            OnScoreChanged?.Invoke(Score);
        }

        public void SetGameEnd(bool win)
        {
            IsGamePlay = false;
            IsWin = win;

            OnGameEnd?.Invoke(win);
        }
    }
}