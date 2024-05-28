using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine
{
    public abstract class EnemyStateTransition : EnemyStateMachineBehaviour
    {
        [SerializeField] private bool _targetDefault;
        [SerializeField, HideIf(nameof(_targetDefault))] private EnemyState _targetState;
        [SerializeField] private bool _anyTransitionState;
        [SerializeField, HideIf(nameof(_anyTransitionState))] private List<EnemyState> _listenStates;
      
        protected bool isListening { get; private set; } = false;

        private void OnEnable()
        {
            if (_anyTransitionState)
            {
                TransitionStartListen();
                return;
            }

            foreach (var state in _listenStates)
            {
                state.entered.On(OnTransitionStateEntered);
                state.exited.On(OnTransitionStateExited);
                if (state.isEntered)
                {
                    OnTransitionStateEntered(state);
                }
            }
        }

        private void OnDisable()
        {
            if (_anyTransitionState)
            {
                TransitionEndListen();
                return;
            }
            foreach (var state in _listenStates)
            {
                state.entered.Off(OnTransitionStateEntered);
                state.exited.Off(OnTransitionStateExited);
            }
        }

        private void OnTransitionStateEntered(EnemyState enemyState)
        {
            TransitionStartListen();
        }

        private void OnTransitionStateExited(EnemyState enemyState)
        {
            if(_listenStates.Has(x=>x.isEntered))
                return;
            TransitionEndListen();
        }

        private void TransitionStartListen()
        {
            isListening = true;
            OnTransitionStartListen();
        }

        private void TransitionEndListen()
        {
            isListening = false;
            OnTransitionEndListen();
        }
        
        protected abstract void OnTransitionStartListen();
        protected abstract void OnTransitionEndListen();
        
        public void Transit()
        {
            if(isListening == false)
                return;
            if (_targetDefault)
            {
                stateMachine.ExitDefault();
                return;
            }
            stateMachine.EnterState(_targetState);
        }
        
    }
}