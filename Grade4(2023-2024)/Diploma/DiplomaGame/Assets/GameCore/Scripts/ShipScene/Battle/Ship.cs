using System.Collections.Generic;
using System.Linq;
using GameCore.ShipScene.Enemies.Spawner;
using GameBasicsCore.Tools.Extensions;
using UnityEngine;

namespace GameCore.ShipScene.Battle
{
    public class Ship : MonoBehaviour
    {
        private EnemySpawnPoint[] _spawnPointsCached;
        private EnemySpawnPoint[] allSpawnPoints =>
            _spawnPointsCached ??= GetComponentsInChildren<EnemySpawnPoint>(true);

        public IEnumerable<EnemySpawnPoint> spawnPoints => allSpawnPoints.Where(x => x.gameObject.activeInHierarchy);
    }
}