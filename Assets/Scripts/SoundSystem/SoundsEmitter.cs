using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Util;
using UnityEngine;

namespace Asteroids.SoundSystem
{
    public class SoundEmitter : MonoBehaviour
    {
        private Dictionary<string, AudioSource> _playing;

        void Awake()
        {
            _playing = new Dictionary<string, AudioSource>();
        }

        public void Play(string path, string soundName = null, float delay = 0f)
        {
            var sound = SoundLoader.Get(path);

            if (!sound)
            {
                Debug.LogWarning($"Failed to get sound {path}");
                return;
            }

            var source = GetSource(path);
            InitSource(source, sound);
            RemoveTimer(source, delay);

            if (delay > 0)
            {
                source.playOnAwake = false;
                source.PlayDelayed(delay);
            }
            else
            {
                source.Play();
            }

            if (soundName != null)
            {
                _playing.Add(soundName, source);
            }
        }

        public void Stop(string soundName, float delay = 0f)
        {
            if (!_playing.ContainsKey(soundName)) return;

            var source = _playing[soundName];
            if (delay > 0f)
            {
                Destroy(source.gameObject, delay);
                return;
            }

            Destroy(source.gameObject);
        }

        private AudioSource GetSource(string soundName)
        {
            var obj = new GameObject("SoundEmitter: " + soundName);
            obj.transform.parent = gameObject.transform;

            return obj.AddComponent<AudioSource>();
        }

        private void InitSource(AudioSource source, SoundData sound)
        {
            source.clip = ArrayUtil.GetRandomItem<AudioClip>(sound.clips);
            source.pitch = ArrayUtil.GetRandomRangeFromArray(sound.pitch);
            source.volume = sound.volume;
            source.spatialBlend = (float)sound.soundType;
            source.outputAudioMixerGroup = sound.mixerGroup;
            source.loop = sound.looping;
        }

        // After the sound is done playing we don't really need it. Looping sound have to be stopped through StopSound()
        private void RemoveTimer(AudioSource source, float delay)
        {
            if (!source.loop)
            {
                Destroy(source.gameObject, delay + source.clip.length);
            }
        }
    }
}