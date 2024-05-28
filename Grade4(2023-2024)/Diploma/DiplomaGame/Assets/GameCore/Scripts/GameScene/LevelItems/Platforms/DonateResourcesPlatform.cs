using System.Collections.Generic;
using DG.Tweening;
using GameCore.Common.Sounds.Api;
using GameCore.Common.UI;
using GameCore.GameScene.Audio;
using GameCore.GameScene.Controllers.ObjectContext;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Saves;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Models;
using GameBasicsCore.Game.Models.GameCurrencies;
using GameBasicsCore.Game.Presenters;
using GameBasicsCore.Game.Presenters.Currencies;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Units.Modules;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Platforms
{
    public abstract class DonateResourcesPlatform : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public BuyCollectingController buyCollectingController { get; }
        [Inject, UsedImplicitly] public PlayerSoftCurrencyCollectModel softCurrencyCollectModel { get; }
        [Inject, UsedImplicitly] public BunchPopUpsMoveAnimationPresenter bunchPopUpsMoveAnimationPresenter { get; }
        [Inject, UsedImplicitly] public BuyItemSound buyItemSound { get; }
        [Inject, UsedImplicitly] public CollectingModule collectingModule { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        [InjectOptional, UsedImplicitly] public ISoftCurrencyUISfxPlayer softCurrencyUISfxPlayer { get; }
        [InjectOptional, UsedImplicitly] public SoftCurrencyPresenter softCurrencyPresenter { get; }
        
        [SerializeField] private int _priceSoftCurrency;
        public int priceSoftCurrency => _priceSoftCurrency;
        [SerializeField] protected List<PriceProduct> _priceList;
        [SerializeField] private Sprite _iconItem;
        public Sprite iconItem => _iconItem;
        [SerializeField] private Transform _popUpPoint;
        [SerializeField] private ProductsCarrier _carrierPrefab;
        [SerializeField] private Transform _carriersContainer;
        [SerializeField] private ProductsCarrier _woodCarrier;

        private List<ProductsCarrier> _carriers = new List<ProductsCarrier>();
        
        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }
        
        public virtual PaidWoodSaveProperty paidWoodSaveProperty { get; }
        public virtual PaidProductsSaveData paidProductsSaveData { get; }
        public virtual PaidSCSaveProperty paidScSaveProperty { get; }
        
        
        private int _getSoftCurrency;
        private int _getWood;

        protected virtual bool canCompleteDonationOnValidate => true;
        
        private bool _isSCCollecting = false;

        private BuyPopUp _buyPopUp;
        
        public TheSignal onCompletedDonation { get; } = new();

         protected virtual void Validate(bool isToUseSaves = false)
        {
            if (isToUseSaves)
            {
                _getWood = paidWoodSaveProperty.value;
                _getSoftCurrency = paidScSaveProperty.value;
            }
            
            _woodCarrier.SetStaticCapacity(0);
            
            foreach (var priceProduct in _priceList)
            {
                if (priceProduct.config.viewPrefab is not WoodProduct)
                {
                    var carrier = Instantiate(_carrierPrefab, _carriersContainer);
                    _carriers.Add(carrier);
                    carrier.SetAcceptableDataConfigs(new List<ProductDataConfig>() { priceProduct.config });
                    int cnt = 0;
                    if (isToUseSaves)
                    {
                        cnt = paidProductsSaveData.value.GetProductCount(priceProduct.config.id);
                    }
                    priceProduct.SetCount(cnt);
                    carrier.SetStaticCapacity(priceProduct.price - cnt);
                    
                    collectingModule.AddProductCarrier(carrier);
                }
                else
                {
                    _woodCarrier.SetStaticCapacity(priceProduct.price - _getWood);
                    priceProduct.SetCount(_getWood);
                    _carriers.Add(_woodCarrier);
                }
            }

            if (isToUseSaves)
            {
                if (IsBought())
                {
                    if (canCompleteDonationOnValidate)
                    {
                        ResetPlatform();
                    }
                    else
                    {
                        DOVirtual.DelayedCall(0.5f, DonateComplete, false).SetLink(gameObject);
                    }
                }
            }
        }

        public void Activate()
        {
            ActivateInternal();
            transform.localScale = Vector3.one * 0.01f;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
        }
        
        public void ActivateInternal(bool isToUseSaves = false)
        {
            Validate(isToUseSaves);
            
            gameObject.SetActive(true);
            interactItem.enabled = true;
            
            SetupBuyPopUp();
            
            foreach (var carrier in _carriers)
            {
                carrier.ReleaseAllProducts();
            }

            buyCollectingController.onAdd.On(OnProductAdded);
        }

        private void OnProductAdded(ProductView product)
        {
            if (product is WoodProduct)
            {
                _priceList.Find(item => item.config.viewPrefab is WoodProduct).IncreaseCount(1);
                    
                _getWood++;
                paidWoodSaveProperty.value++;
                (product as WoodProduct).onSpend.Dispatch();
            }
            else
            {
                _priceList.Find(item => item.config.id == product.id).IncreaseCount(1);
                paidProductsSaveData.value.AddPaidProductCount( product.id, 1);
            }

            AfterGetSomeProduct();
        }
        
        public void AfterGetSomeProduct()
        {
            _buyPopUp.PunchScale();
                
            ValidatePrice();
            if (IsBought())
            {
                DonateComplete();
            }
        }

        public void TryUseSC()
        {
            if (softCurrencyCollectModel == null) return;
            if (softCurrencyCollectModel.earned <= 0) return;
            if (_isSCCollecting) return;
            _isSCCollecting = true;
            
            int amount = _priceSoftCurrency - _getSoftCurrency;
            if (amount > 0)
            {
                if (amount > softCurrencyCollectModel.earned)
                {
                    amount = (int)softCurrencyCollectModel.earned;
                }
                
                softCurrencyCollectModel.Use(amount, "BuyRaftPart", "BuyRaftPart");
                int cntBunches = amount;
                
                if (cntBunches > 30)
                {
                    cntBunches = 30;
                }
                int toAdd = amount / cntBunches;

                int addAtTheEnd = amount - (toAdd * cntBunches);
                
                bunchPopUpsMoveAnimationPresenter.AddArea(_buyPopUp.scArea);
                var animationModel = bunchPopUpsMoveAnimationPresenter.Spawn(_buyPopUp.scArea.id, cntBunches,
                    softCurrencyPresenter.collectedDisplay.GetIconPosition(),
                    _buyPopUp.transform.position);
                animationModel.onChange.On(code =>
                {
                    if (code == BunchPopUpsAnimationModel.ItemComplete)
                    {
                        _getSoftCurrency += toAdd;
                        paidScSaveProperty.value += toAdd;
                        _buyPopUp.PunchScale();
                        ValidatePrice();
                        softCurrencyUISfxPlayer?.PlayCollected();
                    } 
                    else if (code == BunchPopUpsAnimationModel.ItemSpawn)
                    {
                        softCurrencyUISfxPlayer?.PlaySpawned();
                    }
                    else if (code == BunchPopUpsAnimationModel.Complete)
                    {
                        _getSoftCurrency += addAtTheEnd;
                        paidScSaveProperty.value += addAtTheEnd;
                        
                        ValidatePrice();
                        if (IsBought())
                        {
                            DonateComplete();
                        }
                        
                        bunchPopUpsMoveAnimationPresenter.RemoveArea(_buyPopUp.scArea);
                        _isSCCollecting = false;
                    }
                });
            }
            else
            {
                _isSCCollecting = false;
            }
        }

        private bool IsBought()
        {
            foreach (var priceProduct in _priceList)
            {
                if (priceProduct.price > priceProduct.count) return false;
            }
            return _getSoftCurrency >= _priceSoftCurrency;
        }

        public void Deactivate()
        {
            _buyPopUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                _buyPopUp.gameObject.SetActive(false);
                ResetCarrier();
            }).SetLink(gameObject);
            transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).SetLink(gameObject);
            
            buyCollectingController.onAdd.Off(OnProductAdded);
        }
        
        public void DeactivateInternal()
        {
            if (_buyPopUp!=null)
            {
                _buyPopUp.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }

        private void SetupBuyPopUp()
        {
            _buyPopUp = popUpsController.SpawnUnderMenu<BuyPopUp>();
            _buyPopUp.worldSpaceConverter.updateMethod = UpdateMethod.LateUpdate;
            _buyPopUp.worldSpaceConverter.followWorldObject = _popUpPoint;
            _buyPopUp.transform.localScale = Vector3.zero;
            _buyPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);

            _buyPopUp.SetItemIcon(_iconItem);
            
            ValidatePrice();
        }

        private void ValidatePrice()
        {
            if (_priceSoftCurrency > 0 && _getSoftCurrency != _priceSoftCurrency)
            {
                _buyPopUp.SetPrice(_priceList,  _getSoftCurrency+"/" +_priceSoftCurrency);
            }
            else
            {
                _buyPopUp.SetPrice(_priceList);
            }
        }

        public virtual void DonateComplete()
        {
            interactItem.StopInteraction(mainCharacterView.model);
            interactItem.enabled = false;
            ResetPlatform();
            buyItemSound.sound.Play();
            onCompletedDonation.Dispatch();
            Deactivate();
        }

        private void ResetPlatform()
        {
            paidWoodSaveProperty.value = 0;
            paidScSaveProperty.value = 0;
            paidProductsSaveData.value.Clear();
            _getWood = 0;
            _getSoftCurrency = 0;
        }

        private void ResetCarrier()
        {
            foreach (var carrier in _carriers)
            {
                carrier.ReleaseAllProducts();
            }
        }
    }
}