using UnityEngine;
using UnityEngine.Audio;

namespace Asteroids.SoundSystem
{
    [CreateAssetMenu(fileName = "NewSoundData", menuName = "ScriptableObjects/Sound Data")]
    public class SoundData : ScriptableObject
    {
        public enum SoundType
        {
            TwoDimensional,
            ThreeDimensional,
        }

        public AudioClip[] clips;
        public SoundType soundType = SoundType.TwoDimensional;
        public bool looping = false;

        [Range(0f, 1f)] public float volume = 1f;
        public float[] pitch = new float[] { 1 };

        public AudioMixerGroup mixerGroup;

        // 3d settings

        public float minDistance = 1f;
        public float maxDistance = 30f;
    }
}