using System.Collections.Generic;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Controllers;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Items.HittableItems
{
    public class ProductingHittableItem : HittableItem
    {
        [Inject, UsedImplicitly] public UpgradesController upgradesController { get; }
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public ProductionController productionController { get; }
        
        [SerializeField] private ProductDataConfig _spawnProductConfig;
        public ProductDataConfig spawnProductConfig => _spawnProductConfig;
        [SerializeField] private UpgradePropertyDataConfig _spawnCountPerHitConfig;
        [SerializeField] private Transform _pointForSpawn;
        public Transform pointForSpawn => _pointForSpawn;
        [SerializeField] private float _spawnPosRange = 0.5f;
        public float spawnPosRange => _spawnPosRange;
        protected int spawnCountPerHit => upgradesController.GetModel(_spawnCountPerHitConfig).valueInt;
        protected override float enableDelay => 0.5f;

        private List<SellProduct> _spawnedProducts = new List<SellProduct>();
        public List<SellProduct> spawnedProducts => _spawnedProducts;
        
        public override void OnHit(float multipler = 1)
        {
            base.OnHit(multipler);
            SpawnProduction((int) (spawnCountPerHit * multipler * productionController.productionMultiplier));
        }
        
        protected virtual void SpawnProduction(int cnt)
        {
            var spawned = spawnProductsManager.SpawnBunchAtPoint(_spawnProductConfig, _pointForSpawn.position + Vector3.up * 0.1f,
                cnt, _spawnPosRange);
            
            foreach (var prod in spawned)
            {
                if (prod is SellProduct)
                {
                    _spawnedProducts.Add(prod as SellProduct);
                    (prod as SellProduct).onAddedToCarrier.Once(() =>
                    {
                        _spawnedProducts.Remove(prod as SellProduct);
                    });
                }
            }
        }
    }
}