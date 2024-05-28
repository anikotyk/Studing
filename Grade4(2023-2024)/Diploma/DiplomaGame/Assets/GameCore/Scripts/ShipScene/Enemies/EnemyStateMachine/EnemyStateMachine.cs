using System;
using System.Collections.Generic;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine
{
    public class EnemyStateMachine : InjCoreMonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private EnemyState _firstState;
        [SerializeField] private EnemyState _defaultState;
        public Enemy enemy => _enemy;
        public EnemyState currentState { get; private set; }

        private void OnEnable()
        {
            EnterState(_firstState);
        }

        private void OnDisable()
        {
            currentState = null;
        }

        public void EnterState(EnemyState enemyState)
        {
            if(enemyState == currentState)
                return;
            
            if (currentState != null)
                currentState.Exit();
            currentState = enemyState;
            enemyState.Enter();
        }

        public void ExitDefault()
        {
            EnterState(_defaultState);
        }
    }
}