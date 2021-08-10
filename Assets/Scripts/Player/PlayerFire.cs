using System.Collections;
using System.Collections.Generic;
using Asteroids.SoundSystem;
using UnityEngine;

namespace Asteroids.Player
{
    public class PlayerFire : MonoBehaviour
    {
        private ParticleSystem _particle;
        private SoundsEmitter _sound;
        private AudioSource _loop;

        void Awake()
        {
            _sound = GetComponent<SoundsEmitter>();
            _particle = GetComponent<ParticleSystem>();
        }

        void Start()
        {
            _loop = _sound.Play("ShipLoop");
            Update();
        }

        void Update()
        {
            if (!_particle.isPlaying)
            {
                if (_loop.isPlaying)
                {
                    _loop.Pause();
                }
            }
            else if (!_loop.isPlaying)
            {
                _loop.Play();
            }
        }
    }
}