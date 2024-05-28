using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Saves;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems
{
    public class StartWood : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public TakenStartWoodsSaveData takenStartWoodsSaveData { get; }
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        
        private List<WoodProduct> _woodProductsCached;
        public List<WoodProduct> woodProducts
        {
            get
            {
                if (_woodProductsCached == null) _woodProductsCached = GetComponentsInChildren<WoodProduct>().ToList();
                return _woodProductsCached;
            }
        }

        private int _pickedUpCnt;
        
        public readonly TheSignal onUsedAllWoods = new();

        public override void Construct()
        {
            base.Construct();
            ValidateWoods();
        }
        
        private void ValidateWoods()
        {
            for (int i = 0; i < woodProducts.Count; i++)
            {
                if (takenStartWoodsSaveData.value.IsUsed(i))
                {
                    woodProducts[i].gameObject.SetActive(false);
                    _pickedUpCnt++;
                }
            }
        }
        
        private void Start()
        {
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
                    takenStartWoodsSaveData.value.SetUsed(woodProducts.IndexOf(woodProduct));
                });
                spawnProductsManager.SubscribeToRelease(woodProduct);
            }
        }
        
        private void OnEnable()
        {
            foreach (var woodProduct in woodProducts)
            {
                woodProduct.transform.localScale = Vector3.one * 0.01f;
                woodProduct.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            }
        }
    }
}