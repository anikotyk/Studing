using System;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.ShipScene.Damage
{
    public abstract class DamageListener : InjCoreMonoBehaviour
    {
        [SerializeField] private Health _health;

        public Health health => _health;
        
        private void OnEnable()
        {
            _health.damaged.On(OnDamaged);
        }

        private void OnDisable()
        {
            _health.damaged.Off(OnDamaged);
        }

        protected abstract void OnDamaged(int damage);
    }
}