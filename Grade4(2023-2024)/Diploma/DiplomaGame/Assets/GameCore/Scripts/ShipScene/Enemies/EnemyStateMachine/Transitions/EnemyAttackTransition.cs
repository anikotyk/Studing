using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine.Transitions
{
    [RequireComponent(typeof(Collider))]
    public class EnemyAttackTransition : EnemyStateTransition
    {
        [SerializeField] private List<HealthTargetType> _triggerTypes;
        private Collider _colliderCached;

        public List<Health> _healthInRange = new();

        protected Collider Collider
        {
            get
            {
                if (_colliderCached == null)
                    _colliderCached = GetComponent<Collider>();
                return _colliderCached;
            }
        }

        private void OnDisable()
        {
            _healthInRange.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Health health))
            {
                if(health == enemy.health)
                    return;
                if(_triggerTypes.CanApplyDamage(health) == false)
                    return;
                _healthInRange.Add(health);
                if(isListening)
                    Transit();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Health health))
            {
                _healthInRange.Remove(health);
            }
        }

        protected override void OnTransitionStartListen()
        {
            if (_healthInRange.Count > 0)
            {
                Transit();
            }
        }

        protected override void OnTransitionEndListen()
        {
        }
    }
}