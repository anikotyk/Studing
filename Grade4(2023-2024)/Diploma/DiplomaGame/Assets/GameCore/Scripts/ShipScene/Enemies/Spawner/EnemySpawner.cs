using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.ShipScene.Battle;
using JetBrains.Annotations;
using NaughtyAttributes;
using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameCore.ShipScene.Enemies.Spawner
{
    public class EnemySpawner : InjCoreMonoBehaviour
    {
        [SerializeField] private Ship _ship;
        [SerializeField] private List<SpawnItem> _delayedSpawnItem;
        [SerializeField] private Transform _spawnParent;
        [SerializeField] private float _returnDelay;
        [SerializeField] private float _startSpawnDelay;
        [Inject, UsedImplicitly] public DiContainer container { get; }
        [Inject, UsedImplicitly] public BattleController battleController { get; }

        [System.Serializable]
        public class SpawnItem
        {
            public float delay;
            public bool spawnFirstWithoutDelay;
            public Enemy enemyPrefab;
            public int poolSize;
        }

        public List<Enemy> spawnedEnemy { get; } = new();


        private List<SimplePool<Enemy>> _enemiesPool;

        public List<SimplePool<Enemy>> enemiesPool
        {
            get
            {
                InitPools();
                return _enemiesPool;
            }
        }

        public TheSignal<Enemy> spawned { get; } = new();
        public TheSignal<Enemy> killed { get; } = new();
        public TheSignal spawnStarted { get; } = new();
        public TheSignal spawnStopped { get; } = new();

        public override void Construct()
        {
            InitPools();
            battleController.restarted.On(OnRestarted);
        }

        private void OnRestarted()
        {
            foreach (var enemy in spawnedEnemy)
            {
                enemy.pool.Return(enemy);
            }
            spawnedEnemy.Clear();
            EndSpawning();
        }
        
        private void InitPools()
        {
            if (_enemiesPool != null)
                return;
            
            _enemiesPool = new();
            foreach (var spawnItem in _delayedSpawnItem)
            {
                var pool = new SimplePool<Enemy>(spawnItem.enemyPrefab, spawnItem.poolSize, _spawnParent);
                pool.Initialize(container);
                _enemiesPool.Add(pool);
            }
        }
        
        [Button()]
        public void StartSpawning()
        {
            spawnStarted.Dispatch();
            DOVirtual.DelayedCall(_startSpawnDelay, () =>
            {
                DOTween.Kill(this);
                foreach (var spawnItem in _delayedSpawnItem)
                {
                    Spawn(spawnItem);
                }
            });
        }

        private void Spawn(SpawnItem spawnItem)
        {
            if (spawnItem.spawnFirstWithoutDelay)
                Spawn(spawnItem.enemyPrefab);
            DOVirtual.DelayedCall(spawnItem.delay, () => Spawn(spawnItem.enemyPrefab))
                .OnComplete(() => Spawn(spawnItem)).SetId(this);
        }

        [Button()]
        public void EndSpawning()
        {
            DOTween.Kill(this);
            spawnStopped.Dispatch();
        }


        [Button()]
        private void DebugSpawn()
        {
            Spawn(_delayedSpawnItem[0].enemyPrefab);
        }
        
        private Enemy Spawn(Enemy enemyPrefab)
        {
            Enemy enemy;
            var enemyPool = enemiesPool.Find(pool => pool.prefab.id == enemyPrefab.id);
            if (enemyPool != null)
                enemy = enemyPool.Get();
            else
                enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, _spawnParent);
            spawnedEnemy.Add(enemy);
            spawned.Dispatch(enemy);
            enemy.health.died.Once(() => OnKilled(enemy));
            _ship.spawnPoints?.ToList().Random().AddEnemy(enemy);
            return enemy;
        }

        private void OnKilled(Enemy enemy)
        {
            spawnedEnemy.Remove(enemy);
            DOVirtual.DelayedCall(_returnDelay, ()=>enemy.pool.Return(enemy));
            killed.Dispatch(enemy);
        }
    }
}