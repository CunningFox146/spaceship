using System.Collections;
using System.Collections.Generic;
using Asteroids.Asteroid;
using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Player.UI
{
    public class WaveDisplay : MonoBehaviour
    {
        private Text _text;

        void Awake()
        {
            _text = GetComponent<Text>();
        }

        void Start()
        {
            var spawner = AsteroidsSpawner.Inst;

            spawner.OnWaveChanged += OnWaveChangedHandler;
            //OnWaveChangedHandler(spawner.Wave);
        }

        private void OnWaveChangedHandler(int wave)
        {
            AudioManager.Inst.GetSound().Play("UI/WaveChanged");
            _text.text = $"WAVE {wave + 1}";
        }
    }
}
