using DG.Tweening;
using GameCore.GameScene_Island.Controllers.ObjectContext;
using GameCore.GameScene.LevelItems.Platforms;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Platforms
{
    public class HelperStoragePlatform : InteractPlatform
    {
        [Inject, UsedImplicitly] public HelperStorageCollectingController collectingController { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        [SerializeField] private ProductsCarrier _productsCarrier;
        public  ProductsCarrier productsCarrier => _productsCarrier;
        [SerializeField] private bool _isToShowPopUpCount;
        [SerializeField] private Transform _popUpPoint;
        
        private TextPopUpView _spacePopUp;
        private bool _isPopUpShown;
        private Tween _popUpTween;
        private Tween _popUpHideTween;
        
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

            productsCarrier.onChange.On(() =>
            {
                if(_isDestroyed || _spacePopUp == null || !gameObject.activeInHierarchy) return;
                _spacePopUp.SetText(productsCarrier.count + "/" + productsCarrier.capacity);
                if (!productsCarrier.HasSpace())
                {
                    _spacePopUp.SetText("MAX");
                }
                else
                {
                    if (_popUpHideTween != null)
                    {
                        _popUpHideTween.Kill();
                    }
                    _popUpHideTween = DOVirtual.DelayedCall(2f, () =>
                    {
                        HidePopUp();
                    }).SetLink(gameObject);
                }
                if (!_isPopUpShown)
                {
                    ShowPopUp();
                }
            });
        }

        private bool _isDestroyed;
        protected override void OnDestroy()
        {
            base.OnDestroy();

            _isDestroyed = true;
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
    }
}