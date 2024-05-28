using System;
using System.Collections.Generic;
using GameCore.ShipScene.Enemies.Combat;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine
{
    [RequireComponent(typeof(Collider))]
    public abstract class EnemyAttack : InjCoreMonoBehaviour
    {
        [SerializeField] private List<HealthTargetType> _targetTypes;
        [SerializeField] private bool _stopMovement;

        private Enemy _enemyCached;

        public Enemy enemy
        {
            get
            {
                if (_enemyCached == null)
                    _enemyCached = GetComponentInParent<Enemy>();
                return _enemyCached;
            }
        }

        public List<Health> currentTargets { get; } = new();
        public List<Health> startAttackStoredTargets { get; } = new();

        public bool isAttacking { get; private set; } = false;

        private void OnDisable()
        {
            currentTargets.Clear();
            startAttackStoredTargets.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Health health))
            {
                if(_targetTypes.CanApplyDamage(health) == false)
                    return;
                if(currentTargets.Contains(health))
                    return;
                currentTargets.Add(health);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Health health))
            {
                currentTargets.Remove(health);
            }
        }

        public void Attack()
        {
            isAttacking = true;
            if(_stopMovement)
                enemy.DisableMovement(this);
            startAttackStoredTargets.Clear();
            startAttackStoredTargets.AddRange(currentTargets);
            OnAttack();
            enemy.attackModule.attacked.Once(OnAttackComplete);
        }

        public void EndAttack()
        {
            isAttacking = false;
            if(_stopMovement)
                enemy.EnableMovement(this);
            enemy.attackModule.attacked.Off(OnAttackComplete);
            startAttackStoredTargets.Clear();
            OnEndAttack();
        }

        protected void DealDamageToStored(int damage)
        {
            foreach (var target in startAttackStoredTargets)
            {
                target.Damage(damage);
            }
        }

        protected void DealDamageToCurrent(int damage)
        {
            foreach (var target in currentTargets)
            {
                target.Damage(damage);
            }
        }
        
        protected abstract void OnAttack();
        protected virtual void OnAttackComplete(){}
        protected abstract void OnEndAttack();
    }
}