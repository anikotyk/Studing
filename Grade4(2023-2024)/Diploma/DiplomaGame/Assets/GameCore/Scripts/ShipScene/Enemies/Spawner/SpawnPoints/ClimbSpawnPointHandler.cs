using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.ShipScene.Enemies.Spawner;
using GameCore.ShipScene.Enemies.Spawner.SpawnPoints;
using GameCore.ShipScene.Extentions;
using GameBasicsCore.Game.Misc;
using GameBasicsSignals.Api;
using StaserSDK.Extentions;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.Spawning
{
    public class ClimbSpawnPointHandler : SpawnPointHandler
    {
        [SerializeField] private float _climbDuration;
        [SerializeField] private Transform _climbDestination;
        [SerializeField] private ReversibleAnimatorApplier _climbAnimation;
        [Header("Jump")]
        [SerializeField] private AnimatorParameterApplier _jumpAnimation;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpDuration;

        protected override void OnEnemyAdded(Enemy enemy)
        {
            enemy.DisableMovement(this);
            _climbAnimation.SetAnimator(enemy.animator);
            _climbAnimation.Apply();
            var enemyTransform = enemy.transform;
            enemy.died.Once(x =>
            {
                DOTween.Kill(x);
                _climbAnimation.Revert();
            });
            enemyTransform.DOMove(_climbDestination.position, _climbDuration)
                .OnComplete(() =>
                {
                    _climbAnimation.SetAnimator(enemy.animator);
                    _climbAnimation.Revert();
                    
                    var rotationDestination = Vector3.Scale(enemyTransform.rotation.eulerAngles, Vector3.up);
                    enemyTransform.DORotate(rotationDestination, _jumpDuration);
                    enemyTransform.DOJumpY(spawnPoint.destinationPoint.position, _jumpHeight, _jumpDuration)
                        .OnComplete(()=>enemy.EnableMovement(this));
                    
                    _jumpAnimation.SetAnimator(enemy.animator);
                    _jumpAnimation.Apply();
                })
                .SetId(enemy);
        }
    }
}