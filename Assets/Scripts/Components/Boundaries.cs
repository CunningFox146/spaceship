using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Components
{
    public class Boundaries : MonoBehaviour
    {
        [SerializeField] private Vector3 _leftBottomPoint;
        [SerializeField] private Vector3 _rightTopPoint;
        [SerializeField] private float _offset = 1f;

        void Update()
        {
            var pos = transform.position;
            if (pos.x < _leftBottomPoint.x || pos.x > _rightTopPoint.x || 
                (pos.z < _leftBottomPoint.z || pos.z > _rightTopPoint.z))
            {
                var teleportPos = CalculateNewPos(pos);
                if (teleportPos != pos)
                {
                    transform.position = teleportPos;
                }
            }
        }

        // Maybe there should be some sort of better implementation for that but idk
        private Vector3 CalculateNewPos(Vector3 pos)
        {
            if (pos.x < _leftBottomPoint.x)
            {
                return new Vector3(_rightTopPoint.x - _offset, 0, pos.z);
            }
            if (pos.x > _rightTopPoint.x)
            {
                return new Vector3(_leftBottomPoint.x + _offset, 0, pos.z);
            }
            if (pos.z < _leftBottomPoint.z)
            {
                return new Vector3(pos.x, 0, _rightTopPoint.z - _offset);
            }
            if (pos.z > _rightTopPoint.z)
            {
                return new Vector3(pos.x, 0, _leftBottomPoint.z + _offset);
            }

            return pos;
        }
    }
}

