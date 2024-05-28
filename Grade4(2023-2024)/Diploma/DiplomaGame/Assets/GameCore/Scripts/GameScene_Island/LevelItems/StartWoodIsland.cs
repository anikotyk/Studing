using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.Saves;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems
{
    public class StartWoodIsland : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public TakenStartWoodsIslandSaveData takenStartWoodsIslandSaveData { get; }
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }

        [SerializeField] private GameObject _container;
        [SerializeField] private BuyObject _objToRemoveAll;
        
        private List<WoodProduct> _woodProductsCached;
        public List<WoodProduct> woodProducts
        {
            get
            {
                if (_woodProductsCached == null) _woodProductsCached = GetComponentsInChildren<WoodProduct>(true).ToList();
                return _woodProductsCached;
            }
        }

        private int _pickedUpCnt;
        
        private TheSaveProperty<bool> _watchedCutsceneIslandArriveSaveProperty;
        
        public readonly TheSignal onUsedAllWoods = new();

        public override void Construct()
        {
            base.Construct();
            ValidateWoods();
            
            foreach (var woodProduct in woodProducts)
            {
                woodProduct.Initialize(hub);
                woodProduct.onSpend.Once(() =>
                {
                    _pickedUpCnt++;
                    if (_pickedUpCnt >= woodProducts.Count)
                    {
                        onUsedAllWoods.Dispatch();
                    }
                    takenStartWoodsIslandSaveData.value.SetUsed(woodProducts.IndexOf(woodProduct));
                });
                spawnProductsManager.SubscribeToRelease(woodProduct);
            }

            ValidateActive();
            
            initializeInOrderController.Add(Initialize, 10000);
        }

        private void Initialize()
        {
            if (_objToRemoveAll.isBought)
            {
                RemoveAll();
            }
            else
            {
                _objToRemoveAll.onBuy.Once(() =>
                {
                    RemoveAll();
                });
            }
        }
        
        private void ValidateWoods()
        {
            for (int i = 0; i < woodProducts.Count; i++)
            {
                if (takenStartWoodsIslandSaveData.value.IsUsed(i))
                {
                    woodProducts[i].gameObject.SetActive(false);
                    _pickedUpCnt++;
                }
            }
        }

        private void ValidateActive()
        {
            _watchedCutsceneIslandArriveSaveProperty = new(CommStr.WatchedCutsceneArrive_Island, linkToDispose: gameObject);
            if (!_watchedCutsceneIslandArriveSaveProperty.value)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        }

        public void Activate()
        {
            _container.gameObject.SetActive(true);
            foreach (var woodProduct in woodProducts)
            {
                woodProduct.transform.localScale = Vector3.one * 0.01f;
                woodProduct.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    woodProduct.StartSeaIdleAnim(false);
                }).SetLink(gameObject);
            }
        }
        
        public void Deactivate()
        {
            _container.gameObject.SetActive(false);
        }

        private void RemoveAll()
        {
            _container.gameObject.SetActive(false);
            
            foreach (var woodProduct in woodProducts)
            {
                takenStartWoodsIslandSaveData.value.SetUsed(woodProducts.IndexOf(woodProduct));
            }
        }
    }
    
}