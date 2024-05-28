using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.DataConfigs;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Saves;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Managers
{
    public class StackSaveManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [SerializeField] private ProductsCarrier _carrier;
        [SerializeField] private StackSaveIdDataConfig _stackSaveId;
        
        private StackSaveData _stackSaveData;
        
        public override void Construct()
        {
            base.Construct();
            if(!_carrier || !_stackSaveId) return;
            _stackSaveData = new StackSaveData(_stackSaveId.id);
            _stackSaveData.LinkToDispose(gameObject);
            
            SpawnFromSaves();
            
            _carrier.onChange.On(() =>
            {
                _stackSaveData.value.SetProducts(_carrier.products);
            });
        }

        private void SpawnFromSaves()
        {
            List<string> productsIds = _stackSaveData.value.GetProductsIds();
            foreach (string productId in productsIds)
            {
                var config = GameplaySettings.def.stackProducts.FirstOrDefault(config => config.id == productId);
                if (config != null)
                {
                    ProductView prod = spawnProductsManager.Spawn(config);
                    prod.transform.SetParent(_carrier.container);
                    if(prod is SellProduct)
                    {
                        (prod as SellProduct).ResetView();
                    }

                    _carrier.Add(prod, false);
                }
            }
        }

        public void ResetSaves()
        {
            DOVirtual.DelayedCall(0.1f, () =>
            {
                _stackSaveData.Delete();
                ClearStack();
                _stackSaveData.Delete();
            }, false).SetLink(gameObject);
        }

        private void ClearStack()
        {
            foreach (var prod in _carrier.products.ToArray())
            {
                _carrier.GetOut(prod);
                Destroy(prod.gameObject);
            }
        }
    }
}