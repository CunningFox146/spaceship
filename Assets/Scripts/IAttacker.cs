using System;
using UnityEngine;

namespace Scripts
{
    public interface IAttacker
    {
        public void OnAttacked(Collision collision);
    }
}
