using System;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using Pathfinding;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine.Movement
{
    public class EnemyMovementModule : InjCoreMonoBehaviour
    {
        [SerializeField] private AnimatorParameterApplier _speedApplier;
        [SerializeField] private float _maxSpeed = 3;
        [SerializeField] private AIPath _aiPath;
        [SerializeField] private float _sensibility;
        [SerializeField] private float _minSpeedRatio;

        private Vector3 _previousPosition;

        private void OnEnable()
        {
            _previousPosition = _aiPath.transform.position;
        }

        private void FixedUpdate()
        {
            var aiPathPosition = _aiPath.transform.position;
            float moveDistance = VectorExtentions.SqrDistance(aiPathPosition, _previousPosition);
            float speed = moveDistance / Mathf.Pow(_maxSpeed * Time.fixedDeltaTime, 2);
            if (speed > _sensibility && speed < _minSpeedRatio)
                speed = _minSpeedRatio;
            _speedApplier.ApplyAsFloat(Mathf.Clamp01(speed));
            _previousPosition = aiPathPosition;
        }
    }
}