using System;
using System.Collections;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems
{
    public class FishingRod : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public ProductionController productionController { get; }
        
        [SerializeField] private AnimatorParameterApplier _fishingApplier;
        [SerializeField] private LimitedProductStorage _storage;
        [SerializeField] private ProductsCarrier _carrier;
        public ProductsCarrier carrier => _carrier;
        [SerializeField] private Transform _pointSpawnFish;
        [SerializeField] private SellProduct _fishPrefab;
        [SerializeField] private float _customIntervalFishing = -1f;
        [SerializeField] private bool _firstWithoutDelay = false;

        public float intervalFishing => _customIntervalFishing > 0? _customIntervalFishing : GameplaySettings.def.fishingData.intervalFishing;

        private Coroutine _fishingCoroutine;
        
        public void Initialize(FishingRod fishingRod)
        {
            _storage = fishingRod._storage;
            _carrier = fishingRod._carrier;
            _pointSpawnFish = fishingRod._pointSpawnFish;
            _fishPrefab = fishingRod._fishPrefab;
        }
        
        private void OnEnable()
        {
            _fishingCoroutine = StartCoroutine(FishingCoroutine());
        }

        private void OnDisable()
        {
            if (_fishingCoroutine != null)
            {
                StopCoroutine(_fishingCoroutine);
            }
        }

        private IEnumerator FishingCoroutine()
        {
            bool isFirst = true;
            while (true)
            {
                if (!_firstWithoutDelay || !isFirst)
                {
                    yield return new WaitForSeconds(intervalFishing);
                }

                isFirst = false;
                if (_carrier.HasSpace())
                {
                    GetFish();
                }
            }
        }

        private void GetFish()
        {
            StartCoroutine(GetFishCoroutine());
        }

        private IEnumerator GetFishCoroutine()
        {
            _fishingApplier.Apply();
            yield return new WaitForSeconds(0.25f);
            int cnt = (int)(1 * productionController.productionMultiplier);
            for (int i = 0; i < cnt; i++)
            {
                var carrier = _carrier;
                if (!carrier.HasSpace(true)) break;
                SellProduct fish = SpawnFish();
                carrier.Add(fish, true, () =>
                {
                    _storage.Add(fish, false);
                    fish.TurnOnOutline();
                    fish.onAddedToCarrier.Once(() =>
                    {
                        if (carrier.Contains(fish))
                        {
                            carrier.products.Remove(fish);
                            carrier.onChange.Dispatch();
                        }
                    });
                });
            }
          
        }

        private SellProduct SpawnFish()
        {
            var fish = spawnProductsManager.Spawn(_fishPrefab.dataConfig); 
            fish.transform.SetParent(transform);
            fish.transform.position = _pointSpawnFish.position;
            (fish as SellProduct).TurnOffInteractItem();

            return (fish as SellProduct);
        }
    }
}