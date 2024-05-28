using DG.Tweening;
using GameCore.GameScene.Controllers.ObjectContext;
using GameCore.GameScene.LevelItems;
using GameCore.GameScene.LevelItems.Platforms;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Platforms
{
    public class SellFunicularPlatform : InteractPlatform
    {
        [SerializeField] private ProductsCarrier _carrier;
        [SerializeField] private Transform _moveObject;
        [SerializeField] private Transform _movePoint;
        [SerializeField] private Transform _returnPoint;
        [SerializeField] private Transform _popUpPoint;
        [SerializeField] private float _speed;
        
        [Inject, UsedImplicitly] public SellCollectingController sellCollectingController { get; }
        [Inject, UsedImplicitly] public StallObject stallObject { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponentInChildren<InteractItem>();
                return _interactItemCached;
            }
        }

        private bool _isMoving;
        private TextPopUpView _spacePopUp;
        
        private Tween _moveTween;
        private bool _isPopUpShown;
        
        private Tween _popUpTween;
        public readonly TheSignal onEmpty = new();

        public override void Construct()
        {
            base.Construct();
            
            SpawnPopUp();
            
            _carrier.onChange.On(() =>
            {
                _spacePopUp.SetText(_carrier.count +"/"+_carrier.capacity);
                if (!_carrier.HasSpace())
                {
                    _spacePopUp.SetText("FULL");
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        HidePopUp();
                    }).SetLink(gameObject);
                    MoveToSell();
                }
                else if (_carrier.IsEmpty())
                {
                    onEmpty.Dispatch();
                }
                else
                {
                    if (!_isPopUpShown)
                    {
                        ShowPopUp();
                    }
                }
            });
        }

        private void MoveToSell()
        {
            if (_isMoving) return;
            _isMoving = true;
            
            interactItem.enabled = false;
            float time = Vector3.Distance(_moveObject.position, _movePoint.position) / _speed;
            _moveTween = _moveObject.DOMove(_movePoint.position, time).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                onEmpty.Once(() =>
                {
                    MoveBack();
                });
                
                Sell();
            }).SetLink(gameObject);
        }
        
        private void Sell()
        {
            stallObject.sellPlatform.sellCollectingController.Interact(_carrier);
        }
        
        private void MoveBack()
        {
            stallObject.sellPlatform.sellCollectingController.StopInteraction(_carrier);
            float time = Vector3.Distance(_moveObject.position, _returnPoint.position) / _speed;
            _moveTween = _moveObject.DOMove(_returnPoint.position, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                EndMoveBack();
            }).SetLink(gameObject);
        }

        private void EndMoveBack()
        {
            _isMoving = false;
            interactItem.enabled = true;
        }

        private void SpawnPopUp()
        {
            _spacePopUp = popUpsController.SpawnUnderMenu<TextPopUpView>("SpacePopUp");
            _spacePopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
            _spacePopUp.worldSpaceConverter.followWorldObject = _popUpPoint;
            _spacePopUp.transform.localScale = Vector3.zero;
            _spacePopUp.SetText("0/"+_carrier.capacity);
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
            _spacePopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
    }
}