using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Components
{
    public class BoundsManager : Singleton<BoundsManager>
    {
        private Dictionary<GameObject, IBoundsTrackable> _inBoundsList;
        
        public float BoundsWidth { get; private set; }
        public float BoundsHeight { get; private set; }

        public override void Awake()
        {
            base.Awake();

            _inBoundsList = new Dictionary<GameObject, IBoundsTrackable>();

            RecalculateBounds();
        }
        
        private void Update() // TODO maybe it's better to use LateUpdate without LINQ?
        {
            foreach (GameObject obj in _inBoundsList.Keys.ToList()) // Maybe this is not that efficient, but modification-friendly
            {
                _inBoundsList[obj].UpdateBounds();
            }
        }

        public static bool GetInBounds(Vector3 pos, float offset)
        {
            var self = BoundsManager.Inst;

            float boundsWidth = self.BoundsWidth;
            float boundsHeight = self.BoundsHeight;

            return !(pos.x < (-boundsWidth - offset * 0.5f) || (pos.x > (boundsWidth + offset * 0.5f)) ||
                     pos.z < (-boundsHeight - offset * 0.5f) || pos.z > (boundsHeight + offset * 0.5f));
        }

        public static Vector3 GetNewPos(Vector3 pos, float offset)
        {
            var self = BoundsManager.Inst;

            float boundsWidth = self.BoundsWidth;
            float boundsHeight = self.BoundsHeight;

            if (pos.x < (-boundsWidth - offset * 0.5f))
            {
                return new Vector3(pos.x + boundsWidth * 2, pos.y, pos.z);
            }
            if (pos.x > (boundsWidth + offset * 0.5f))
            {
                return new Vector3(pos.x - boundsWidth * 2, pos.y, pos.z);
            }
            if (pos.z < (-boundsHeight - offset * 0.5f))
            {
                return new Vector3(pos.x, pos.y, pos.z + boundsHeight * 2);
            }
            if (pos.z > (boundsHeight + offset * 0.5f))
            {
                return new Vector3(pos.x, pos.y, pos.z - boundsHeight * 2);
            }

            return pos;
        }

        public void Add(GameObject obj)
        {
            var trackable = obj.GetComponent<IBoundsTrackable>();
            if (trackable != null && !_inBoundsList.ContainsKey(obj))
            {
                _inBoundsList.Add(obj, trackable);
            }
        }

        public void Remove(GameObject obj) => _inBoundsList.Remove(obj);
        
        private void RecalculateBounds()
        {
            var cam = Camera.main;
            var point = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            BoundsWidth = point.x;
            BoundsHeight = point.z;
        }
    }
}