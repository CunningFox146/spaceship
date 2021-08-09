using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Player.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _heartPrefab;
        [SerializeField] private float _spacing = 2;
        
        private Heart[] _hearts;
        private int _maxHealth;

        public void Init(int healthAmount)
        {
            _hearts = new Heart[healthAmount];
            _maxHealth = healthAmount;

            float heartWidth = 32f;
            for (int i = 0; i < healthAmount; i++)
            {
                var child = Instantiate(_heartPrefab, transform);
                var heart = child.GetComponent<Heart>();
                var rect = child.transform as RectTransform;
                if (rect != null)
                {
                    rect.anchoredPosition = new Vector2(-(_spacing + heartWidth) * (healthAmount - 1) * 0.5f + i * (heartWidth + _spacing), 0);
                }

                _hearts[i] = heart;
            }
        }

        public void SetHealth(int healthAmount)
        {
            for (int i = healthAmount; i < _maxHealth; i++)
            {
                _hearts[i].SetFull(false);
            }
        }

    }
}
