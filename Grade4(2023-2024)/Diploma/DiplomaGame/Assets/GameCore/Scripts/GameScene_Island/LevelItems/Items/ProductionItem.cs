using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.UI;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class ProductionItem : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public ProductionController productionController { get; }
        
        [SerializeField] private LimitedProductStorage _productStorage;
        [SerializeField] private Transform _productsUsePoint;
        [SerializeField] private ProductDataConfig _spawnProductConfig;
        [SerializeField] private float _timeWorking;
        [SerializeField] private Transform _popUpPoint;
        [SerializeField] private List<PriceProductCarrier> _priceList;
        public List<PriceProductCarrier> priceList => _priceList;
        
        private ProductPopUp _productPopUp;

        protected bool _isWorking;

        public override void Construct()
        {
            base.Construct();
            initializeInOrderController.Add(Initialize, 2000);
        }

        private void Initialize()
        {
            BuyObject buyObject = GetComponentInParent<BuyObject>();
            if (buyObject && !buyObject.isBought)
            {
                buyObject.onBuy.Once(SetWorkingPopUp);
            }
            else
            {
                SetWorkingPopUp();
            }
            
            foreach (var item in _priceList)
            {
                item.carrier.onChange.On(() =>
                {
                    SetWorkingPopUp();
                });
                item.carrier.onAddComplete.On((_, _) =>
                {
                    OnAddedObjectToPlatform();
                });
            }
        }

        public bool CanProduct()
        {
            foreach (var item in _priceList)
            {
                if (item.count < item.price) return false;
            }

            return true;
        }

        protected virtual void OnAddedObjectToPlatform()
        {
            if (!_isWorking && CanProduct())
            {
                StartWorking();
            }
        }

        public virtual void StartWorking()
        {
            if(_isWorking) return;
            _isWorking = true;
            EffectOnStartWorking();
            StartCoroutine(UseProducts());
            float timer = 0;
            DOVirtual.DelayedCall(_timeWorking, EndWorking, false).OnUpdate(() =>
            {
                timer += Time.deltaTime;
                _productPopUp.SetProgress(timer / _timeWorking);
            }).SetLink(gameObject);
        }

        private void SetWorkingPopUp()
        {
            if (_productPopUp == null)
            {
                if(popUpsController == null || popUpsController.containerUnderMenu == null) return;
                _productPopUp = popUpsController.SpawnUnderMenu<ProductPopUp>("ProductPopUp");
                _productPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
                _productPopUp.worldSpaceConverter.followWorldObject = _popUpPoint.transform;
                _productPopUp.SetItemIcon(_spawnProductConfig.icon);
            }
           
            _productPopUp.transform.localScale = Vector3.one;
            _productPopUp.gameObject.SetActive(true);
            _productPopUp.SetProgress(0);
            _productPopUp.SetPrice(_priceList);
        }
        
        protected IEnumerator UseProducts()
        {
            foreach (var item in _priceList)
            {
                for (int i = 0; i < item.price; i++)
                {
                    var prod = item.carrier.GetOutLast();
                    prod.transform.DOJump(_productsUsePoint.position, 1f, 1, 0.5f).OnComplete(() =>
                    {
                        Destroy(prod.gameObject);
                    }).SetLink(gameObject);
                    
                    yield return new WaitForSeconds(0.15f);
                }
            }
        }

        protected virtual void EffectOnStartWorking()
        {
            
        }
        
        public virtual void EndWorking()
        {
            EffectOnEndWorking();
            int prodCount = (int) productionController.productionMultiplier;
            for (int i = 0; i < prodCount; i++)
            {
                SpawnProduct();
            }
           
            _productPopUp.SetProgress(0);
            _isWorking = false;
            if (CanProduct())
            {
                StartWorking();
            }
        }

        protected virtual void SpawnProduct()
        {
            var prod = spawnProductsManager.Spawn(_spawnProductConfig);
            (prod as SellProduct).TurnOffInteractItem();
            prod.transform.position = _productsUsePoint.position;
            _productStorage.Add(prod, true);
            prod.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        
        protected virtual void EffectOnEndWorking()
        {
            
        }
    }
}