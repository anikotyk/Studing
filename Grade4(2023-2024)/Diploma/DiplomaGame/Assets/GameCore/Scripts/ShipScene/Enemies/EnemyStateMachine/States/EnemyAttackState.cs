using System.Collections.Generic;
using GameCore.ShipScene.Extentions;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine
{
    public class EnemyAttackState : EnemyState
    {
        [SerializeField] private List<EnemyAttack> _attacks;
        [SerializeField] private bool _triggetEventsInternal;

        private EnemyAttack _lastAttack;
        
        protected override void OnEnter()
        {
            _lastAttack = _attacks.Random();
            if(_triggetEventsInternal)
                enemy.attackModule.TriggerStarted();
            _lastAttack.Attack();
        }

        protected override void OnExit()
        {
            if(_lastAttack != null)
                _lastAttack.EndAttack();
            if(_triggetEventsInternal)
                enemy.attackModule.TriggerStopped();
        }
    }
}