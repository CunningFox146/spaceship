using System;
using Asteroids.Asteroid;
using Asteroids.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Asteroids.Player.UI
{
    public class PlayerHud : Singleton<PlayerHud>
    {
        [SerializeField] private GameEndPanel _gameEndPanel;
        [SerializeField] private HitOverlay _hitOverlay;
        [SerializeField] private HealthDisplay _healthDisplay;
        [SerializeField] private Text _nextWave;
        [SerializeField] private Fade _fade;

        private Health _health;

        public override void Awake()
        {
            base.Awake();

            _health = GetComponent<Health>();
        }

        void Start()
        {
            _healthDisplay.Init(_health.maxHealth);
            _fade.Hide(() =>
            {
                _fade.gameObject.SetActive(false);
            });

            AsteroidsSpawner.Inst.OnWaveChanged += OnWaveChangedHandler;
            _health.OnHealthChanged += OnHealthChangedHandler;
            _health.OnDeath += OnDeathHandler;
        }

        // Not sure if it should go here or in player controller
        void Update()
        {
            if (_gameEndPanel.IsDone && Input.anyKeyDown && !_fade.isFading)
            {
                _fade.gameObject.SetActive(true);
                _fade.Show(() =>
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                });
            }
        }

        private void OnWaveChangedHandler(int wave)
        {
            float duration = 0.5f;
            float delay = 1f;
            float offset = 35;
            var startPos = _nextWave.rectTransform.anchoredPosition;

            _nextWave.gameObject.SetActive(true);
            _nextWave.text = wave > 0 ? "NEXT WAVE" : "START!";

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
            Invoke("ShowGameEndPanel", 1.5f);
            _hitOverlay.OnHit();
            _healthDisplay.SetHealth(0);
        }

        private void ShowGameEndPanel()
        {
            AudioManager.Inst.MusicPitchDown();

            _gameEndPanel.gameObject.SetActive(true);
            _gameEndPanel.Init();
        }

        private void OnHealthChangedHandler(int newHealth)
        {
            _hitOverlay.OnHit();
            _healthDisplay.SetHealth(newHealth);
        }
    }
}
