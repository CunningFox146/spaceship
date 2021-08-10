using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Asteroid
{
    [CreateAssetMenu(fileName = "AsteroidWave", menuName = "ScriptableObjects/Asteroid Wave", order = 1)]
    public class AsteroidWave : ScriptableObject
    {
        public int asteroidsCount = 1;
        public int smallAsteroidsCount = 0;
    }
}