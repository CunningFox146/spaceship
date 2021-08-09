using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Components
{
    public class HitOverlay : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _alphaCurve;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void OnHit()
        {
            _image.color = Color.white;
            LeanTween.value(1f, 0f, 0.75f).setOnUpdate((float val) => _image.color = new Color(1, 1, 1, val)).setEaseOutCubic();
        }
    }
}