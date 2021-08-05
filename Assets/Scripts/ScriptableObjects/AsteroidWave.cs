using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AsteroidWave", menuName = "ScriptableObjects/AsteroidWave", order = 1)]
    public class AsteroidWave : ScriptableObject
    {
        public int asteroidsCount = 1;
        public int smallAsteroidsCount = 0;
    }
}