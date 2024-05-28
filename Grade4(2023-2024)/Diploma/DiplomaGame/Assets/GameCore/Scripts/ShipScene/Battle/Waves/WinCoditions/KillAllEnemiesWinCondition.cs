using GameCore.ShipScene.Enemies;
using GameCore.ShipScene.Enemies.Spawner;
using UnityEngine;

namespace GameCore.ShipScene.Battle.Waves.WinCoditions
{
    public class KillAllEnemiesWinCondition : WaveWinCondition
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        
        protected override void StartListen()
        {
            _enemySpawner.spawnStopped.On(OnSpawnStopped);
        }

        private void OnSpawnStopped()
        {
            if(TryEnd())
                return;
            _enemySpawner.killed.On(OnEnemyKilled);
        }

        private void OnEnemyKilled(Enemy enemy)
        {
            TryEnd();
        }

        private bool TryEnd()
        {
            if (_enemySpawner.spawnedEnemy.Count == 0)
            {
                Victory();
                return true;
            }
            return false;
        }

        protected override void EndListen()
        {
            _enemySpawner.spawnStopped.Off(OnSpawnStopped);
            _enemySpawner.killed.Off(OnEnemyKilled);
        }
    }
}