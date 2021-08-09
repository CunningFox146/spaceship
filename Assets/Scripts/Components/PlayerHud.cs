using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Components
{
    public class PlayerHud : Singleton<PlayerHud>
    {
        [SerializeField] private HitOverlay _hitOverlay;
        [SerializeField] private HealthDisplay _healthDisplay;

        private Health _health;

        public override void Awake()
        {
            base.Awake();

            _health = GetComponent<Health>();
        }

        void Start()
        {
            _healthDisplay.Init(_health.maxHealth);


            _health.OnHealthChanged += OnHealthChangedHandler;
            _health.OnDeath += OnDeathHandler;
        }

        private void OnDeathHandler()
        {

            _hitOverlay.OnHit();
            _healthDisplay.SetHealth(0);
        }

        private void OnHealthChangedHandler(int newHealth)
        {
            _hitOverlay.OnHit();
            _healthDisplay.SetHealth(newHealth);
        }
    }
}
