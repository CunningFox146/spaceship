using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Asteroids
{
    [Serializable]
    public enum PoolItem
    {
        Meteor,
        MeteorShard,
        Bullet,
        AudioSource,
    }

    [Serializable]
    public struct ObjectPool
    {
        public GameObject prefab;
        public int count;
        public PoolItem item;   
    }
}
