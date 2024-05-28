using System.Collections.Generic;
using DG.Tweening;
using GameCore.Common.Sounds.Api;
using GameCore.GameScene_Iceland.DataConfigs;
using GameCore.GameScene_Iceland.LevelItems.Items;
using GameCore.GameScene_Island.LevelItems.Platforms;
using GameCore.GameScene.Audio;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc.Refs;
using GameBasicsCore.Game.Models.GameCurrencies;
using GameBasicsCore.Game.Presenters.Currencies;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class EskimosShopCustomersManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public MainCameraRef mainCameraRef { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public PlayerSoftCurrencyCollectModel softCurrencyCollectModel { get; }
        [InjectOptional, UsedImplicitly] public SoftCurrencyPresenter softCurrencyPresenter { get; }
        [InjectOptional, UsedImplicitly] public ISoftCurrencyUISfxPlayer softCurrencyUISfxPlayer { get; }
        
        [SerializeField] private SellProductsCollectPlatform _sellProductsCollectPlatform;
        [SerializeField] private EskimosCustomersGroupDataConfig _cutomersGroupDataConfig;
        [SerializeField] private Transform _moneySpawnPoint;
        [SerializeField] private float _intervalSpawnCustomers;
        [SerializeField] private float _priceMultiplier = 1.5f;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _sellPoint;
        [SerializeField] private Transform _endPoint;
        
        private EskimosCustomerView _currentCustomer;
        private Tween _startCustomerTween = null;

        private List<EskimosCustomerView> _customers = new List<EskimosCustomerView>();
        
        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 20000);
        }

        private void Initialize()
        {
            if (_sellProductsCollectPlatform.productsCarrier.IsEmpty())
            {
                _sellProductsCollectPlatform.productsCarrier.onAddComplete.Once((_, _)=>
                {
                    TryStartCustomer();
                });
            }
            else
            {
                TryStartCustomer();
            }
        }

        private void TryStartCustomer()
        {
            if (_startCustomerTween != null)
            {
                _startCustomerTween.Kill();
            }
            _startCustomerTween = DOVirtual.DelayedCall(_intervalSpawnCustomers, () =>
            {
                if (!_sellProductsCollectPlatform.productsCarrier.IsEmpty())
                {
                    StartCustomer();
                }
                else
                {
                    DOVirtual.DelayedCall(_intervalSpawnCustomers, TryStartCustomer,false).SetLink(gameObject);
                }
            },false).SetLink(gameObject);
        }

        private void StartCustomer(EskimosCustomerView customer = null)
        {
            if (customer == null)
            {
                customer = GetRandomCustomer();
            }
            
            customer.gameObject.SetActive(true);
            customer.StartMove();
            customer.onBoughtProduct.Once((prod)=>
            {
                GetMoney(prod.priceDataConfig.softCurrencyCount);
                TryStartCustomer();
            });
        }

        private EskimosCustomerView GetRandomCustomer()
        {
            int index = Random.Range(0, _cutomersGroupDataConfig.Count);
            EskimosCustomerView customer = _customers.Find((view => view.id == _cutomersGroupDataConfig[index].id && !view.isInWayNow));
            if (customer == null)
            {
                customer = SpawnSeller(index);
            }
            _currentCustomer = customer;
            return customer;
        }
        
        private EskimosCustomerView SpawnSeller(int index)
        {
            EskimosCustomerView customer = Instantiate(_cutomersGroupDataConfig[index].view, transform);
            customer.Initialize(_sellProductsCollectPlatform.productsCarrier, _startPoint, _sellPoint, _endPoint);
            _customers.Add(customer);
            
            return customer;
        }

        public void GetMoney(int payment)
        {
            payment = (int) (payment * _priceMultiplier);
            
            int cntParts = payment;
            int maxCntParts = 50;
            cntParts = cntParts > maxCntParts ? maxCntParts : cntParts;
            int amount = payment / cntParts;
            
            for (int j = 0; j < cntParts; j++)
            {
                softCurrencyPresenter.SpawnAndFlyToCollectDisplay(_moneySpawnPoint.position, onComplete: () =>
                {
                    softCurrencyCollectModel.Earn(amount, "EskimosShop", "EskimosShop", true);
                    softCurrencyUISfxPlayer?.PlayCollected();
                }, radius: 0.5f, camera:mainCameraRef);
            }
        }
    }
}