using System;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.Spawner.SpawnPoints
{
    public abstract class SpawnPointHandler : InjCoreMonoBehaviour
    {
        [SerializeField] private EnemySpawnPoint _enemySpawnPoint;

        public EnemySpawnPoint spawnPoint => _enemySpawnPoint;
        
        private void OnEnable()
        {
            _enemySpawnPoint.added.On(OnEnemyAdded);
        }

        private void OnDisable()
        {
            _enemySpawnPoint.added.Off(OnEnemyAdded);
        }

        protected abstract void OnEnemyAdded(Enemy enemy);
    }
}