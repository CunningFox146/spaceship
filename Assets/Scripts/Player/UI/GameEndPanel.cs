using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Asteroids.Managers;
using UnityEngine;

namespace Asteroids.Player.UI
{
    public class GameEndPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _text;

        private CanvasGroup _canvas;

        void Start()
        {
            _canvas = GetComponent<CanvasGroup>();

            ScoreManager.Inst.OnGameEnd += GameEndHandler;
        }

        private void GameEndHandler(bool obj)
        {
            LeanTween.alphaCanvas(_canvas, 1f, 0.5f).setEase(LeanTweenType.easeOutBack);
        }
    }
}