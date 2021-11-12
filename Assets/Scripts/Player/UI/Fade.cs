using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Player.UI
{
    public class Fade : MonoBehaviour
    {
        public bool isFading;
        private CanvasGroup _canvas;

        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
        }

        public void Show(Action onComplete = null)
        {
            isFading = true;
            _canvas.alpha = 0f;
            var tween = LeanTween.alphaCanvas(_canvas, 1f, 0.5f).setEaseInCubic();
            if (onComplete != null)
            {
                tween.setOnComplete(onComplete);
            }
        }

        public void Hide(Action onComplete = null)
        {
            _canvas.alpha = 1f;
            var tween = LeanTween.alphaCanvas(_canvas, 0f, 0.5f).setEaseInCubic();
            if (onComplete != null)
            {
                tween.setOnComplete(onComplete);
            }
        }
    }
}