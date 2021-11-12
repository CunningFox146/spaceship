using System;
using UnityEngine;

namespace Asteroids.Player
{
    public class Health : MonoBehaviour
    {
        public event Action<int> OnHealthChanged;
        public event Action OnDeath;

        [SerializeField] public int maxHealth = 3;

        private int _currentHealth;
        private bool isDead = false;

        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                if (_currentHealth > 0)
                {
                    OnHealthChanged?.Invoke(_currentHealth);
                }
            }
        }

        void Start()
        {
            _currentHealth = maxHealth;
        }

        public void DoDelta(int delta)
        {
            if (isDead) return;

            CurrentHealth = Mathf.Max(CurrentHealth + delta, 0);
            if (CurrentHealth == 0)
            {
                isDead = true;
                OnDeath?.Invoke();
            }
        }
    }
}
