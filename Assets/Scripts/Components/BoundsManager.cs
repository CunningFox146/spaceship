using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class BoundsManager : Singleton<BoundsManager>
    {
        [SerializeField] private float _offset; // Temp

        public Vector3 MinPos;
        public Vector3 MaxPos;

        private List<GameObject> _inBoundsList;

        public override void Awake()
        {
            base.Awake();

            _inBoundsList = new List<GameObject>();
        }

        public void Track(GameObject obj)
        {
            if (!_inBoundsList.Contains(obj))
            {
                _inBoundsList.Add(obj);
            }
        }

        public void StopTracking(GameObject obj)
        {
            _inBoundsList.Remove(obj);
        }

        public bool IsInBounds(GameObject obj) => _inBoundsList.Contains(obj);
        
        public void Teleport(GameObject obj)
        {
            if (!IsInBounds(obj)) return;

            var pos = obj.transform.position;
            var teleportPos = CalculateNewPos(pos);

            if (teleportPos != pos)
            {
                Debug.Log($"Teleport {obj}");
                obj.transform.position = teleportPos;
            }

            //Track(obj);
        }

        private Vector3 CalculateNewPos(Vector3 pos)
        {
            if (pos.x < MinPos.x)
            {
                return new Vector3(MaxPos.x - _offset, 0, pos.z);
            }

            if (pos.x > MaxPos.x)
            {
                return new Vector3(MinPos.x + _offset, 0, pos.z);
            }

            if (pos.z < MinPos.z)
            {
                return new Vector3(pos.x, 0, MaxPos.z - _offset);
            }

            if (pos.z > MaxPos.z)
            {
                return new Vector3(pos.x, 0, MinPos.z + _offset);
            }

            return pos;
        }
    }
}