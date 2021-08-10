using System.Collections;
using System.Collections.Generic;
using Asteroids.Asteroid;
using Asteroids.SoundSystem;
using UnityEngine;
using UnityEngine.Audio;

namespace Asteroids.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        private const float MinPitch = 0.5f;
        private const float MaxPitch = 1f;

        [SerializeField] private AudioMixer _mixer;

        private SoundsEmitter _sound;
        private float _musicPitch;

        public SoundsEmitter GetSound() => _sound;

        public override void Awake()
        {
            if (AudioManager.Inst != null)
            {
                Destroy(gameObject);
            }

            base.Awake();
            
            DontDestroyOnLoad(gameObject);

             _mixer.GetFloat("MusicPitch", out _musicPitch);

            _sound = GetComponent<SoundsEmitter>();

            if (_musicPitch < MaxPitch)
            {
                MusicPitchUp();
            }
        }

        void Start()
        {
            _sound.Play("MainTheme");
        }

        public void MusicPitchUp()
        {
            if (_musicPitch >= 1f) return;

            LeanTween.value(_musicPitch, MaxPitch, 1f).setOnUpdate((float val) =>
            {
                _musicPitch = val;
                UpdatePitch();
            });
        }

        public void MusicPitchDown()
        {
            if (_musicPitch <= 0f) return;

            LeanTween.value(_musicPitch, MinPitch, 1f).setOnUpdate((float val) =>
            {
                _musicPitch = val;
                UpdatePitch();
            });
        }
        
        private void UpdatePitch()
        {
            _mixer.SetFloat("MusicPitch", _musicPitch);
        }
    }
}
