using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    struct ObjectPool
    {
        public GameObject prefab;
        public int count;
    }
}
