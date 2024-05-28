using System;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine
{
    public class EnemyState : EnemyStateMachineBehaviour
    {
        public TheSignal<EnemyState> entered { get; } = new();
        public TheSignal<EnemyState> exited { get; } = new();

        public bool isEntered { get; private set; } = false;

        protected void OnEnable()
        {
        }

        protected void OnDisable()
        {
            if(enemy.isInjected == false)
                return;
            Exit();
        }

        public void Enter()
        {
            EnterInternal();
        }
        
        private void EnterInternal()
        {
            enabled = true;
            isEntered = true;
            entered.Dispatch(this);
            OnEnter();
        }
        protected virtual void OnEnter(){}

        public void Exit()
        {
            ExitInternal();
        }
        
        private void ExitInternal()
        {
            enabled = false;
            isEntered = false;
            exited.Dispatch(this);
            OnExit();
        }
        protected virtual void OnExit(){}
    }
}