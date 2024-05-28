using System;
using GameCore.ShipScene.Battle.Waves;
using GameCore.ShipScene.Enemies.Spawner;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Battle
{
    public class WaveSpawnStarter : InjCoreMonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private Wave _wave;

        private void OnEnable()
        {
            _wave.timerStarted.On(StartSpawn);
            _wave.timerEnded.On(_enemySpawner.EndSpawning);
            _wave.ended.On(_enemySpawner.EndSpawning);
        }

        private void StartSpawn()
        {
            _enemySpawner.StartSpawning();
        }

        private void OnDisable()
        {
            _wave.started.Off(StartSpawn);
            _wave.timerEnded.Off(_enemySpawner.EndSpawning);
        }
    }
}