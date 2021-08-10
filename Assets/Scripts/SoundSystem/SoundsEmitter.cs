using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Managers;
using Asteroids.Util;
using UnityEngine;

namespace Asteroids.SoundSystem
{
    public class SoundsEmitter : MonoBehaviour
    {
        private Dictionary<string, AudioSource> _playing;

        void Awake()
        {
            _playing = new Dictionary<string, AudioSource>();
        }

        public AudioSource Play(string path, string soundName = null, float delay = 0f)
        {
            var sound = SoundLoader.Get(path);

            if (!sound)
            {
                Debug.LogWarning($"Failed to get sound {path}");
                return null;
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

            return source;
        }

        public void Stop(string soundName, float delay = 0f)
        {
            if (!_playing.ContainsKey(soundName)) return;

            var source = _playing[soundName];
            if (delay > 0f)
            {
                DestroySource(source, delay);
                return;
            }

            DestroySource(source);
        }

        private AudioSource GetSource(string soundName)
        {
            var obj = ObjectPooler.Inst.Get(PoolItem.AudioSource);
            obj.name = "SoundEmitter: " + soundName;
            obj.transform.parent = gameObject.transform;

            return obj.GetComponent<AudioSource>();
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
                DestroySource(source, delay + source.clip.length);
            }
        }

        private void DestroySource(AudioSource source, float delay = 0f)
        {
            void DestroyObj()
            {
                source.Stop();
                ObjectPooler.Inst.Return(PoolItem.AudioSource, source.gameObject);
            }

            if (delay > 0f)
            {
                LeanTween.delayedCall(delay, DestroyObj);
                return;
            }

            DestroyObj();
        }
    }
}