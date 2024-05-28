using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using JetBrains.Annotations;
using NaughtyAttributes;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using Pathfinding;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Products
{
    public class ProductSpawner : InjCoreMonoBehaviour
    {
        [SerializeField] private ProductDataConfig _productDataConfig;
        [SerializeField] private Vector3 _spawnDelta;
        [SerializeField] private float _jumpRange;
        [SerializeField] private bool _checkAstar;
        [SerializeField] private bool _selfTargetPoint = true;
        [SerializeField, HideIf(nameof(_selfTargetPoint))] private Transform _target;
        [SerializeField] private float _spawnDelay;
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }

        private Transform target => _selfTargetPoint ? transform : _target;

        [Button()]
        public void Spawn()
        {
            Spawn(1);
        }

        public void Spawn(int count)
        {
            Vector3 startPosition = target.position + _spawnDelta;

            for (int i = 0; i < count; i++)
            {
                Vector3 position = startPosition + (Random.insideUnitCircle * _jumpRange).XZ();
                if (_checkAstar)
                    position = (Vector3)AstarPath.active.GetNearest(position, new NNConstraint()).node.position;

                DOVirtual.DelayedCall(_spawnDelay * i, () =>
                {
                    spawnProductsManager.SpawnBunchAtPoint(_productDataConfig, position, 1, 0);
                });
            }
            
        }
    }
}