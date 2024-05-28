using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using GameCore.Common.Sounds.Api;
using GameCore.GameScene.Audio;
using GameCore.GameScene.DataConfigs;
using GameCore.GameScene.LevelItems.Sellers;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using NaughtyAttributes;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc.Refs;
using GameBasicsCore.Game.Models.GameCurrencies;
using GameBasicsCore.Game.Presenters.Currencies;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class SellersManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public StallObject stallObject { get; }
        [Inject, UsedImplicitly] public MainCameraRef mainCameraRef { get; }
        [Inject, UsedImplicitly] public PlayerSoftCurrencyCollectModel softCurrencyCollectModel { get; }
        [InjectOptional, UsedImplicitly] public SoftCurrencyPresenter softCurrencyPresenter { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [InjectOptional, UsedImplicitly] public ISoftCurrencyUISfxPlayer softCurrencyUISfxPlayer { get; }

        [SerializeField] private bool _isStallOverride;
        public bool IsStallOverride() => _isStallOverride;
        [SerializeField] [ShowIf("IsStallOverride")] private StallObject _stallObject ;
        [SerializeField] private SplineComputer _splineComputer;
        [SerializeField] private SellersGroupDataConfig _sellersGroupDataConfig;
        [SerializeField] private SellerView _firstSeller;

        private SellerView _currentSeller;
        public StallObject currentStallObject => IsStallOverride() ? _stallObject : stallObject;

        private bool _isTurnedOff;
        private Tween _startSellerTween = null;

        private List<SellerView> _sellers = new List<SellerView>();
        public GameplaySettings.SellersData sellersData => GameplaySettings.def.sellersData;
        
        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 20000);
        }

        private void Initialize()
        {
            if (!currentStallObject.buyObject || currentStallObject.buyObject.isBought)
            {
                TryStartSeller();
            }
            else
            {
                currentStallObject.buyObject.onBuy.Once(() =>
                {
                    TryStartSeller();
                });
            }
        }

        public void StartFirstSeller()
        {
            if(!_firstSeller) return;
            _isTurnedOff = true;
            _firstSeller.gameObject.SetActive(true);
            _firstSeller.Initialize(_splineComputer, currentStallObject.sellPlatform.sellCollectingController.module.productsCarriers[0], this);
            StartSeller(_firstSeller);
            _firstSeller.onFinishWay.Once(() =>
            {
                _isTurnedOff = false;
            });
        }

        private void TryStartSeller()
        {
            if (_startSellerTween != null)
            {
                _startSellerTween.Kill();
            }
            _startSellerTween = DOVirtual.DelayedCall(sellersData.intervalSpawnSellers, () =>
            {
                if (!_isTurnedOff && currentStallObject.gameObject.activeSelf && !currentStallObject.sellPlatform.sellCollectingController.module.productsCarriers[0].IsEmpty())
                {
                    StartSeller();
                }
                else
                {
                    DOVirtual.DelayedCall(sellersData.intervalSpawnSellers, TryStartSeller,false).SetLink(gameObject);
                }
            },false).SetLink(gameObject);
        }

        private void StartSeller(SellerView seller = null)
        {
            if (seller == null)
            {
                seller = GetRandomSeller();
            }
            
            seller.gameObject.SetActive(true);
            seller.StartMove();
            seller.onMiddleWay.Once(TryStartSeller);
        }

        private SellerView GetRandomSeller()
        {
            int index = Random.Range(0, _sellersGroupDataConfig.Count);
            SellerView seller = _sellers.Find((view => view.id == _sellersGroupDataConfig[index].id && !view.isInWayNow));
            if (seller == null)
            {
                seller = SpawnSeller(index);
            }
            _currentSeller = seller;
            return seller;
        }
        
        private SellerView SpawnSeller(int index)
        {
            SellerView seller = Instantiate(_sellersGroupDataConfig[index].viewPrefab, transform);
            seller.Initialize(_splineComputer, currentStallObject.sellPlatform.sellCollectingController.module.productsCarriers[0], this);
            _sellers.Add(seller);
            
            return seller;
        }

        public void GetMoney(int payment)
        {
            int cntParts = payment;
            int maxCntParts = 50;
            cntParts = cntParts > maxCntParts ? maxCntParts : cntParts;
            int amount = payment / cntParts;
            
            for (int j = 0; j < cntParts; j++)
            {
                softCurrencyPresenter.SpawnAndFlyToCollectDisplay(currentStallObject.sellPlatform.transform.position, onComplete: () =>
                {
                    softCurrencyCollectModel.Earn(amount, "Sellers", "Sellers", true);
                    softCurrencyUISfxPlayer?.PlayCollected();
                }, radius: 0.5f, camera:mainCameraRef);
            }
        }

        public void TurnOffSellers()
        {
            gameObject.SetActive(false);
            if (_currentSeller != null)
            {
                _currentSeller.TurnOff();
            }

            _isTurnedOff = true;
        }
        
        public void TurnOnSellers()
        {
            gameObject.SetActive(true);
            _isTurnedOff = false;
        }
    }
}