using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class BoundsManager : Singleton<BoundsManager>
    {
        public Vector3 minPos;
        public Vector3 maxPos;

        public float worldWidth = 0f;
        public float worldHeight = 0f;

        private List<GameObject> _inBoundsList;

        public override void Awake()
        {
            base.Awake();

            _inBoundsList = new List<GameObject>();

            RecalculateBounds();
        }

        private void RecalculateBounds()
        {
            var cam = Camera.main;
            var point = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            worldWidth = point.x;
            worldHeight = point.y;

            GameObject.CreatePrimitive(PrimitiveType.Capsule).transform.position =
                new Vector3(worldWidth * 0.5f, 0f, worldHeight * 0.5f);
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
                return new Vector3(maxPos.x, pos.y, pos.z);
            }

            if (pos.x > maxPos.x)
            {
                return new Vector3(minPos.x, pos.y, pos.z);
            }

            if (pos.z < minPos.z)
            {
                return new Vector3(pos.x, pos.y, maxPos.z);
            }

            if (pos.z > maxPos.z)
            {
                return new Vector3(pos.x, pos.y, minPos.z);
            }

            return pos;
        }
    }
}