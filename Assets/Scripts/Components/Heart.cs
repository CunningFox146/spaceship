using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Components
{
    public class Heart : MonoBehaviour
    {
        [SerializeField] private Sprite _spriteFull;
        [SerializeField] private Sprite _spriteEmpty;

        private Image _image;
        private RectTransform _rect;
        private bool _isFull = true;

        void Awake()
        {
            _image = GetComponent<Image>();
            _rect = GetComponent<RectTransform>();
        }

        public void SetFull(bool isFull)
        {
            if (_isFull == isFull) return;
            _image.sprite = isFull ? _spriteFull : _spriteEmpty;
        }
    }

}
