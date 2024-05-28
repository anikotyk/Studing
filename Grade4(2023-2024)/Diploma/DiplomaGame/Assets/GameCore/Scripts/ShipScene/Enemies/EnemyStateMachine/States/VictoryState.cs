using GameCore.ShipScene.Extentions;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine
{
    public class VictoryState : EnemyState
    {
        [SerializeField] private ReversibleAnimatorApplier _reversibleAnimatorApplier;

        protected override void OnEnter()
        {
            enemy.DisableMovement(this);
            _reversibleAnimatorApplier.Apply();
        }

        protected override void OnExit()
        {
            enemy.EnableMovement(this);
            _reversibleAnimatorApplier.Revert();
        }
    }
}