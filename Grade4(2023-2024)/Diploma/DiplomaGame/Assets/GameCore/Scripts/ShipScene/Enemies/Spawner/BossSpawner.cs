using System;
using DG.Tweening;
using GameCore.ShipScene.Battle;
using GameCore.ShipScene.Battle.Waves;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Enemies.Spawner
{
    public class BossSpawner : InjCoreMonoBehaviour
    {
        [SerializeField] private Enemy _boss;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private EnemySpawner _spawner;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private float _enableMovementDelay = 2.0f;

        [Inject, UsedImplicitly] public BattleController battleController { get; }
        
        private void OnEnable()
        {
            _boss.gameObject.SetActive(false);
            _spawner.spawnStarted.On(OnSpawnStarted);
        }

        public override void Construct()
        {
            battleController.restarted.On(OnRestarted);
        }
        

        private void OnDisable()
        {
            _spawner.spawnStarted.Off(OnSpawnStarted);
        }

        private void OnRestarted()
        {
            _boss.gameObject.SetActive(false);
            _boss.health.Revive();
        }

        private void OnSpawnStarted()
        {
            DOVirtual.DelayedCall(_spawnDelay, () =>
            {
                _boss.DisableMovement(this);
                DOVirtual.DelayedCall(_enableMovementDelay, () => _boss.EnableMovement(this));
                _boss.transform.position = _spawnPoint.position;
                _boss.gameObject.SetActive(true);
            });
        }
    }
}