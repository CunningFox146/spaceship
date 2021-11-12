using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Player
{
    public class PhoneControls : MonoBehaviour
    {
        [SerializeField] private Text _text;
        private PlayerController _controller;

        void Awake()
        {
            _controller = GetComponent<PlayerController>();
        }

        void Start()
        {
            var platform = Application.platform;
            if (platform != RuntimePlatform.Android)
            {
                enabled = false;
            }
            else
            {
                _controller.isMobile = true;
            }
        }

        void Update()
        {
            float width = Screen.width;
            float height = Screen.height;
            
            _controller.inputH = 0f;
            _controller.inputV = 0f;
            _controller.inputV = 0f;
            _controller.isShooting = false;

            for (int i = 0; i < Input.touchCount; ++i)
            {
                var touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    var pos = touch.position;
                    float w = pos.x / width;
                    float h = pos.y / height;
                    _text.text = $"{w} {h}";
                    if (w <= 0.2f)
                    {
                        _controller.inputH = -1f;
                    }
                    else if (w >= 0.8f)
                    {
                        _controller.inputH = 1f;
                    }
                    else if (h >= 0.5f)
                    {
                        _controller.inputV = 1f;
                    }
                    else
                    {
                        _controller.isShooting = true;
                    }
                }
            }
        }
    }
}