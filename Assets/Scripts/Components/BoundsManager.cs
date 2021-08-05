using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class BoundsManager : Singleton<BoundsManager>
    {
        [SerializeField] private float _offset; // Temp

        public Vector3 minPos;
        public Vector3 maxPos;

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
            //if (!IsInBounds(obj)) return;

            var pos = obj.transform.position;
            var teleportPos = CalculateNewPos(pos);

            if (teleportPos != pos)
            {
                //Debug.Log($"Teleport {obj}");
                obj.transform.position = teleportPos;
            }

            //Track(obj);
        }

        private Vector3 CalculateNewPos(Vector3 pos)
        {
            if (pos.x < minPos.x)
            {
                return new Vector3(maxPos.x - _offset, pos.y, pos.z);
            }

            if (pos.x > maxPos.x)
            {
                return new Vector3(minPos.x + _offset, pos.y, pos.z);
            }

            if (pos.z < minPos.z)
            {
                return new Vector3(pos.x, pos.y, maxPos.z - _offset);
            }

            if (pos.z > maxPos.z)
            {
                return new Vector3(pos.x, pos.y, minPos.z + _offset);
            }

            return pos;
        }
    }
}