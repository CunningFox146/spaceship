using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Components
{
    class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private Transform _camLeft;
        [SerializeField] private Transform _camRight;
        [SerializeField] private Transform _camTop;
        [SerializeField] private Transform _camBottom;

        private Camera _camera;
        private Vector3 _startPos;
        private float _shakeDuration;
        private float _shakeForce;

        private int _screenWidth = 0;
        private int _screenHeight = 0;

        public override void Awake()
        {
            base.Awake();

            _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            UpdateCameras();
        }
        
        private void Update()
        {
            if (_shakeDuration != 0f)
            {
                _shakeDuration = Mathf.Max(_shakeDuration - Time.deltaTime, 0);
                ShakeCamera();
            }

            if (_screenWidth != Screen.width || _screenHeight != Screen.height)
            {
                UpdateCameras();
            }
        }

        private void UpdateCameras()
        {
            var bounds = BoundsManager.Inst;
            bounds.RecalculateBounds();

            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
            
            float width = bounds.BoundsWidth;
            float height = bounds.BoundsHeight;

            _camLeft.localPosition = new Vector3(-width * 2f, 0f, 0f);
            _camRight.localPosition = new Vector3(width * 2f, 0f, 0f);
            _camTop.localPosition = new Vector3(0f, height * 2f, 0f);
            _camBottom.localPosition = new Vector3(0f, -height * 2f, 0f);
        }

        private void ShakeCamera()
        {
            if (_shakeDuration == 0)
            {
                transform.position = _startPos;
                return;
            }

            transform.position = _startPos + Random.insideUnitSphere * _shakeForce;
        }

        public void Shake(float duration, float force)
        {
            _startPos = transform.position;
            _shakeDuration = duration;
            _shakeForce = force;

            Update();
        }
    }
}
