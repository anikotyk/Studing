using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
using Random = UnityEngine.Random;

namespace GameCore.GameScene.LevelItems
{
    public class FishingNet : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public ProductionController productionController { get; }
        
        [SerializeField] private AnimatorParameterApplier _twistApplier;
        [SerializeField] private AnimatorParameterApplier _twistBackApplier;
        
        [SerializeField] private LimitedProductStorage _storage;
        [SerializeField] private ProductsCarrier _carrier;
        [SerializeField] private Transform _containerSpawnFish;
        [SerializeField] private SellProduct _fishPrefab;

        private Coroutine _fishingCoroutine;
        
        private void OnEnable()
        {
            if (_fishingCoroutine != null)
            {
                StopCoroutine(_fishingCoroutine); 
            }
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
            while (true)
            {
                yield return new WaitForSeconds(GameplaySettings.def.fishingData.intervalFishing);
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
            _twistBackApplier.Apply();
            List<SellProduct> fishes = SpawnFishes();
            yield return new WaitForSeconds(1f);
            
            foreach (var fish in fishes)
            {
                var carrier = _carrier;
                carrier.Add(fish ,true, () =>
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
            
            _twistApplier.Apply();
        }

        private List<SellProduct> SpawnFishes()
        {
            int cnt = GameplaySettings.def.fishingData.netFishesCount; 
            cnt = (int)(cnt * productionController.productionMultiplier);
            if (cnt > _carrier.capacity - _carrier.count)
            {
                cnt = _carrier.capacity - _carrier.count;
            }
            
            List<SellProduct> fishes = new List<SellProduct>();
            for (int i = 0; i < cnt; i++)
            {
                fishes.Add(SpawnFish());
            }

            return fishes;
        }

        private SellProduct SpawnFish()
        {
            var fish = spawnProductsManager.Spawn(_fishPrefab.dataConfig);
            fish.transform.SetParent(_containerSpawnFish);
            Vector3 pos = Vector3.zero;
            pos.z += Random.Range(-0.2f, 0.2f);
            fish.transform.localPosition = pos;
            (fish as SellProduct).TurnOffInteractItem();
            return  (fish as SellProduct);
        }
    }
}