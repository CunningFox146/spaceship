﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Components
{
    public class PlayerHud : Singleton<PlayerHud>
    {
        [SerializeField] private HitOverlay _hitOverlay;
        [SerializeField] private HealthDisplay _healthDisplay;
        [SerializeField] private Text _nextWave;

        private Health _health;

        public override void Awake()
        {
            base.Awake();

            _health = GetComponent<Health>();
        }

        void Start()
        {
            _healthDisplay.Init(_health.maxHealth);

            AsteroidsSpawner.Inst.OnWaveChanged += OnWaveChangedHandler;
            _health.OnHealthChanged += OnHealthChangedHandler;
            _health.OnDeath += OnDeathHandler;
        }

        private void OnWaveChangedHandler(int wave)
        {
            float duration = 0.5f;
            float delay = 1f;
            float offset = 35;
            var startPos = _nextWave.rectTransform.anchoredPosition;

            _nextWave.gameObject.SetActive(true);

            _nextWave.rectTransform.anchoredPosition = startPos + new Vector2(-offset, 0f);
            _nextWave.color = new Color(1f, 1f, 1f, 0);

            LeanTween.value(0f, 1f, duration)
                .setEaseInCubic()
                .setOnUpdate((float val)=> _nextWave.color = new Color(1f,1f,1f, val))
                .setOnComplete(() =>
                {
                    LeanTween.value(1f, 0f, duration)
                        .setEaseOutCubic()
                        .setDelay(delay)
                        .setOnUpdate((float val) => _nextWave.color = new Color(1f, 1f, 1f, val))
                        .setOnComplete(() =>
                        {
                            _nextWave.rectTransform.anchoredPosition = startPos;
                            _nextWave.gameObject.SetActive(false);
                        });
                });

            LeanTween.move(_nextWave.rectTransform, startPos, duration)
                .setEaseInCubic()
                .setOnComplete(()=>
                {
                    LeanTween.move(_nextWave.rectTransform, startPos + new Vector2(offset, 0), duration)
                        .setEaseOutCubic()
                        .setDelay(delay);
                });
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
