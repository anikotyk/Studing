using DG.Tweening;
using GameCore.GameScene.Helper;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Helper.Modules
{
    public class HelperSellStorageModule : InteractorCharacterModule
    {
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }

        [SerializeField] private bool _isToShowPopUp;
        [SerializeField] private GameObject _productsStorage;
        public GameObject productsStorage => _productsStorage; 
        [SerializeField] private ProductsCarrier _storageCarrier; 
        public ProductsCarrier storageCarrier => _storageCarrier; 
        [SerializeField] private Transform _moveStoragePoint;
        public Transform moveStoragePoint => _moveStoragePoint; 
        [SerializeField] private Transform _popUpPoint;

        private HelperView _viewCached;
        public HelperView view => _viewCached ??= GetComponentInParent<HelperView>(true);

        private int _prevCount;
        private TextPopUpView _popUpFull;
        private Tween _popUpTween;
        private bool _isShownPopUp;

        public override void Construct()
        {
            base.Construct();

            initializeInOrderController.Add(Initialize, 100000);
            
            if (_isToShowPopUp)
            {
                SetUpPopUp();
            }
        }

        private void Initialize()
        {
            _storageCarrier.onChange.On(() =>
            {
                if (_isDestroyed) return;
                if(_prevCount == _storageCarrier.count) return;
                _prevCount = _storageCarrier.count;
                if (!_storageCarrier.HasSpace())
                {
                    if (!_isShownPopUp && _isToShowPopUp)
                    {
                        ShowPopUp();
                    }
                    
                    if (view.logicModule.isTurnedOn)
                    {
                        view.logicModule.TurnOffHelper();
                    }
                }
                else
                {
                    if (_isShownPopUp && _isToShowPopUp)
                    {
                        HidePopUp();
                    }

                    if (!view.logicModule.isTurnedOn)
                    {
                        view.logicModule.TurnOnHelper();
                    }
                }
            });
        }
        
        private void SetUpPopUp()
        { 
            _popUpFull = popUpsController.SpawnUnderMenu<TextPopUpView>("HelperStorageIsFull");
            _popUpFull.worldSpaceConverter.updateMethod = UpdateMethod.Update;
            _popUpFull.worldSpaceConverter.followWorldObject = _popUpPoint;
            _popUpFull.transform.localScale = Vector3.zero;
        }
        
        private void HidePopUp()
        {
            _isShownPopUp = false;
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _popUpFull.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }

        private bool _isDestroyed;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _isDestroyed = true;
        }
        
        private void ShowPopUp()
        {
            _isShownPopUp = true;
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _popUpFull.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
    }
}