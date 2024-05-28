using Pathfinding;
using StaserSDK.Extentions;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine
{
    public class EnemyDeathState : EnemyState
    {
        [SerializeField] private float _distanceToWalkableNodeToFall;

        protected override void OnEnter()
        {
            enemy.DisableMovement(this);
            var enemyPosition = enemy.transform.position;
            var nearestAstarNode = (Vector3)AstarPath.active.GetNearest(enemyPosition, new NNConstraint())
                .node
                .position;

            float distance = VectorExtentions.SqrDistance(nearestAstarNode.XZ(), enemyPosition.XZ());
            if (distance > _distanceToWalkableNodeToFall * _distanceToWalkableNodeToFall)
            {
                enemy.rigidbody.isKinematic = false;
                enemy.rigidbody.useGravity = true;
            }
            
        }

        protected override void OnExit()
        {
            enemy.rigidbody.isKinematic = true;
            enemy.rigidbody.useGravity = false;
            enemy.EnableMovement(this);
        }
    }
}