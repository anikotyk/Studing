using System;
using UnityEngine;

namespace GameCore.ShipScene.Damage
{
    public abstract class LiveListener : MonoBehaviour
    {
        [SerializeField] private Health _health;

        public bool isDied { get; private set; } = false;
        
        private void OnEnable()
        {
            _health.died.On(Died);
            _health.revived.On(Revived);
        }

        private void OnDisable()
        {
            _health.died.Off(Died);
            _health.revived.Off(Revived);
        }

        private void Died()
        {
            isDied = false;
            OnDied();
        }

        protected abstract void OnDied();

        private void Revived()
        {
            isDied = true;
            OnRevived();
        }
        
        protected abstract void OnRevived();
        
    }
}