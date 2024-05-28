using DG.Tweening;
using GameCore.Common.LevelItems;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class Elevator : InjCoreMonoBehaviour
    { 
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        [SerializeField] private Transform _moveObject;
        [SerializeField] private Transform _upPoint;
        [SerializeField] private Transform _downPoint;
        [SerializeField] private float _speed;

        private Tween _moveTween;
        private Tween _moveCallTween;

        private bool _isDown = true;
        private bool _isTurnedOn = true;

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 10000);
        }
        
        private void Initialize()
        {
            if (!GetComponentInParent<BuyObject>().isBought)
            {
                TurnOff();
                GetComponentInParent<BuyObject>().onBuy.Once(() =>
                {
                    TurnOn();
                });
            }
        }

        private void MoveUp()
        {
            if (_moveTween != null)
            {
                _moveTween.Kill();
            }

            if (_moveCallTween != null)
            {
                _moveCallTween.Kill();
            }
            
            float time = Mathf.Abs(_moveObject.position.y - _upPoint.position.y) / _speed;
            _moveTween = _moveObject.DOMoveY(_upPoint.position.y, time).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed).SetLink(gameObject).OnComplete(
                () =>
                {
                    _isDown = false;
                });
        }

        private void MoveDown()
        {
            if (_moveTween != null)
            {
                _moveTween.Kill();
            }
            if (_moveCallTween != null)
            {
                _moveCallTween.Kill();
            }
            
            float time = Mathf.Abs(_moveObject.position.y - _downPoint.position.y) / _speed;
            _moveTween = _moveObject.DOMoveY(_downPoint.position.y, time).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(
                () =>
                {
                    _isDown = true;
                });
        }

        public void OnCharacterInteract(InteractorCharacterModel interactorModel)
        {
            if(!_isTurnedOn) return;
            if (!_isDown && (_moveTween == null || !_moveTween.IsPlaying()))
            {
                MoveDown();
            }
            else
            {
                MoveUp();
            }
        }

        public void OnCharacterInteractDown(InteractorCharacterModel interactorModel)
        {
            if(!_isTurnedOn) return;
            MoveDown();
        }
        
        public virtual void OnCharacterStopInteract()
        {
            if(!_isTurnedOn) return;
            MoveDown();
        }

        public void TurnOff()
        {
            _isTurnedOn = false;
        }
        
        public void TurnOn()
        {
            _isTurnedOn = true;
        }

        public void OnTopNow()
        {
            _isDown = false;
        }
    }
}