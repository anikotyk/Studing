using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.ShipScene.Extentions;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Tools.Extensions;
using StaserSDK.Extentions;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine.Attacks
{
    public class AnimationMeleeAttack : EnemyAttack
    {
        [SerializeField] private int _damage;
        [SerializeField] private AnimatorParameterApplier _animatorParameterApplier;
        [SerializeField] private float _maxAttackRange;
        [SerializeField, Range(-1, 1)] private float _minDot;
        
        protected override void OnAttack()
        {
            _animatorParameterApplier.Apply();
        }

        protected override void OnAttackComplete()
        {
            var targets = new List<Health>();
            targets.AddRange(currentTargets);
            targets.AddRange(startAttackStoredTargets);
            
            foreach (var target in targets.Distinct())
            {
                if(CanDealDamageToTarget(target) == false)
                    continue;
                target.Damage(_damage);
            }
        }

        private bool CanDealDamageToTarget(Health target)
        {
            var enemyPosition = enemy.transform.position;
            var targetPosition = target.transform.position;
            if(VectorExtentions.SqrDistance(enemyPosition, targetPosition) >= _maxAttackRange * _maxAttackRange)
                return false;
                
            Vector3 relativePositionToTarget = targetPosition - enemyPosition;
            float dot = Vector3.Dot(enemy.transform.forward, relativePositionToTarget);
            if(_minDot > dot)
                return false;
            
            return true;
        }
        
        protected override void OnEndAttack()
        {
        }
    }
}