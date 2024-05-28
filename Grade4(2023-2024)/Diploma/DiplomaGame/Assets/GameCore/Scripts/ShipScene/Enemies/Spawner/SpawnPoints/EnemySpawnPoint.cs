using GameBasicsSignals;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.Spawner
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _startLookAtPoint;
        [SerializeField] private Transform _destinationPoint;

        public Transform startPoint => _startPoint;
        public Transform destinationPoint => _destinationPoint;

        public TheSignal<Enemy> added { get; } = new();

        public void AddEnemy(Enemy enemy)
        {
            enemy.transform.position = startPoint.position;
            enemy.transform.LookAt(_startLookAtPoint);
            added.Dispatch(enemy);
        }
    }
}