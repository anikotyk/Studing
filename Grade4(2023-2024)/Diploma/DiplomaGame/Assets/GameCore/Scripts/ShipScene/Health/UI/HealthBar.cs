using System;
using UnityEngine;

namespace GameCore.ShipScene.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private SimpleSlider _slider;
        [SerializeField] private Health _health;

        private void OnEnable()
        {
            OnHealthChanged();
            _health.healthChanged.On(OnHealthChanged);
        }

        private void OnDisable()
        {
            _health.healthChanged.Off(OnHealthChanged);
        }

        private void OnHealthChanged()
        {
            _slider.value = _health.currentHealth / (float)_health.maxHealth;
        }
    }
}