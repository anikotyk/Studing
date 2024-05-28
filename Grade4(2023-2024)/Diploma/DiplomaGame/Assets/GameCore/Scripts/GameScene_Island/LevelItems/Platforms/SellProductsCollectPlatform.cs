using DG.Tweening;
using GameCore.GameScene.LevelItems.Platforms;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Platforms
{
    public class SellProductsCollectPlatform : InteractPlatform
    {
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        [SerializeField] private ProductsCarrier _productsCarrier;
        public  ProductsCarrier productsCarrier => _productsCarrier;
        
        [SerializeField] private ProductDataConfig _productDataConfig;
        public ProductDataConfig productDataConfig => _productDataConfig;
        
        [SerializeField] private bool _isToShowPopUpCount;
        [SerializeField] private Transform _popUpPoint;

        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }
        
        private TextPopUpView _spacePopUp;
        private bool _isPopUpShown;
        private Tween _popUpTween;
        private Tween _popUpHideTween;
        
        public readonly TheSignal onFull = new();

        public override void Construct()
        {
            base.Construct();
            
            if (_isToShowPopUpCount)
            {
                SetUpPopUp();
            }
        }

        private void SetUpPopUp()
        { 
            _spacePopUp = popUpsController.SpawnUnderMenu<TextPopUpView>("SpacePopUp");
            _spacePopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
            _spacePopUp.worldSpaceConverter.followWorldObject = _popUpPoint;
            _spacePopUp.transform.localScale = Vector3.zero;
            _spacePopUp.SetText("0/"+productsCarrier.capacity);

            productsCarrier.onAddComplete.On((_, _) =>
            {
                _spacePopUp.SetText(productsCarrier.count + "/" + productsCarrier.capacity);
                if (!productsCarrier.HasSpace())
                {
                    onFull.Dispatch();
                    _spacePopUp.SetText("MAX");
                }

                if (!_isPopUpShown)
                {
                    ShowPopUp();
                }

                if (_popUpHideTween != null)
                {
                    _popUpHideTween.Kill();
                }
                _popUpHideTween = DOVirtual.DelayedCall(2f, () =>
                {
                    HidePopUp();
                }).SetLink(gameObject);
            });
        }
        
        private void HidePopUp()
        {
            _isPopUpShown = false;
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _spacePopUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
        
        private void ShowPopUp()
        {
            _isPopUpShown = true;
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _spacePopUp.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }

        public void RemoveAllProducts()
        {
            int cnt = productsCarrier.products.Count;
            for (int i = 0; i < cnt; i++)
            {
                var item = productsCarrier.GetOutLast();
                Destroy(item.gameObject);
            }
        }
    }
}