using DG.Tweening;
using GameCore.Common.Sounds.Api;
using GameCore.Common.UI;
using GameCore.GameScene_Island.LevelItems.InteractItems;
using GameCore.GameScene.Audio;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc.Refs;
using GameBasicsCore.Game.Models.GameCurrencies;
using GameBasicsCore.Game.Presenters.Currencies;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Items
{
    public class SellAllItem : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; } 
        [Inject, UsedImplicitly] public MainCameraRef mainCameraRef { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public PlayerSoftCurrencyCollectModel softCurrencyCollectModel { get; }
        [InjectOptional, UsedImplicitly] public SoftCurrencyPresenter softCurrencyPresenter { get; }
        [InjectOptional, UsedImplicitly] public ISoftCurrencyUISfxPlayer softCurrencyUISfxPlayer { get; }
        
        [SerializeField] private Transform _popUpPoint;
        [SerializeField] private ProductsCarrier _carrier;
        [SerializeField] private CollectFromHelperStorageInteractItem _collectingItem;
        
        private InteractorCharacterModel mainCharacterModel => interactorCharactersCollection.mainCharacterView.model;
        
        private SellAllPopUpView _sellPopUp;
        private Tween _popUpTween;
        private bool _isShown;
        private bool _isCanBeShown;

        public override void Construct()
        {
            base.Construct();

            CreateSellPopUp();
            _carrier.onChange.On(() =>
            {
                if (_isShown && _carrier.products.Count <= 0)
                {
                    HideSellPopUp();
                }else if(_isCanBeShown && !_isShown && _carrier.products.Count > 0)
                {
                    ShowSellPopUp();
                }
            });
        }

        private void CreateSellPopUp()
        {
            _sellPopUp = popUpsController.SpawnUnderMenu<SellAllPopUpView>("SellAllPopUp");
            _sellPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
            _sellPopUp.worldSpaceConverter.followWorldObject = _popUpPoint.transform;
            _sellPopUp.transform.localScale = Vector3.zero;
            _sellPopUp.onClickSellAll.On(SellAll);
            _sellPopUp.gameObject.SetActive(false);
        }

        public void OnCharacterEnter()
        {
            _isCanBeShown = true;
            if (_carrier.products.Count > 0)
            {
                ShowSellPopUp();
            }
        }
        
        public void OnCharacterExit()
        {
            _isCanBeShown = false;
            HideSellPopUp();
        }
        
        private void ShowSellPopUp()
        {
            _isShown = true;
            _sellPopUp.gameObject.SetActive(true);
            
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _popUpTween = _sellPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
          
        private void HideSellPopUp()
        {
            _isShown = false;
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _popUpTween = _sellPopUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                _sellPopUp.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }

        private void SellAll()
        {
           // stallObject.sellPlatform.sellCollectingController.Interact(_carrier);
            
           _collectingItem.collectingController.StopInteraction(mainCharacterModel);
           int amount = 0;
           foreach (var prod in _carrier.products)
           {
               amount += (prod as SellProduct).priceDataConfig.softCurrencyCount;
           }

           _carrier.Clear();

           if (amount > 0)
           {
               GetMoney(amount);
           }
           
           if (_carrier.products.Count <= 0)
           {
               HideSellPopUp();
           }
        }
        
        public void GetMoney(int payment)
        {
            int cntParts = payment;
            int maxCntParts = 50;
            cntParts = cntParts > maxCntParts ? maxCntParts : cntParts;
            int amount = payment / cntParts;
            
            for (int j = 0; j < cntParts; j++)
            {
                softCurrencyPresenter.SpawnAndFlyToCollectDisplay(transform.position, onComplete: () =>
                {
                    softCurrencyCollectModel.Earn(amount, "HelperStorage", "HelperStorage", true);
                    softCurrencyUISfxPlayer?.PlayCollected();
                }, radius: 0.5f, camera:mainCameraRef);
            }
        }
    }
}