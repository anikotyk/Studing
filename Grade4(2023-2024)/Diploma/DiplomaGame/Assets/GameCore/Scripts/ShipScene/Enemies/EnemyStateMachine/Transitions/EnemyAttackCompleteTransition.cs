using DG.Tweening;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine.Transitions
{
    public class EnemyAttackCompleteTransition : EnemyStateTransition
    {
        protected override void OnTransitionStartListen()
        {
            enemy.attackModule.stopped.On(Transit);
        }

        protected override void OnTransitionEndListen()
        {
            enemy.attackModule.stopped.Off(Transit);
        }

    }
}