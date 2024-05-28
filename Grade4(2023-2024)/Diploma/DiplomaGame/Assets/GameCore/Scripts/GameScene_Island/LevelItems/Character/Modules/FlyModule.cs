using System;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.GameScene.LevelItems.Character.Modules;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Character.Modules
{
    public class FlyModule : InteractorCharacterModule
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        [SerializeField] private AnimatorParameterApplier _fly;
        [SerializeField] private AnimatorParameterApplier _notFly;
        [SerializeField] private GameObject _parachute;
        [SerializeField] private Transform _pointForRaycast;
        [SerializeField] private float _distanceStartFly;
        [SerializeField] private float _distanceEndFly;
        [SerializeField] private float _delayStartFly;
        [SerializeField] private float _gravityFly;
        [SerializeField] private BuyObject _buyObjectToAvailable;

        private float _defaultGravity;
        private bool _isFlying = false;
        public bool isFlying => _isFlying;

        private bool _isAvailable = false;

        private Tween _flyEffectTween;
        private Tween _parachuteTween;

        public readonly TheSignal onFly = new();
        public readonly TheSignal onNotFly = new();
        
        private MainCharacterSimpleWalkerController _walkerController;


        private void Awake()
        {
            _walkerController = character.GetComponent<MainCharacterSimpleWalkerController>();
            _defaultGravity = _walkerController.gravity;
        }

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 10000);
        }

        private void Initialize()
        {
            _isAvailable = _buyObjectToAvailable.isBought;
            if (!_isAvailable)
            {
                _buyObjectToAvailable.onBuy.Once(() =>
                {
                    _isAvailable = true;
                });
            }
        }

        public void SetFly()
        {
            if(!_isAvailable) return;
            if (_isFlying) return;
            _isFlying = true;

            DOVirtual.DelayedCall(_delayStartFly, () =>
            {
                if (CheckForStartFly())
                {
                    _fly.Apply();

                    if (_flyEffectTween != null)
                    {
                        _flyEffectTween.Kill();
                    }
                    _flyEffectTween = DOVirtual.DelayedCall(0.1f, () =>
                    {
                        if (_parachuteTween != null)
                        {
                            _parachuteTween.Kill();
                        }
                        _parachute.gameObject.SetActive(true);
                        _parachute.transform.localScale = Vector3.zero;
                        _parachuteTween = _parachute.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack).SetLink(gameObject);
                    }, false).SetLink(gameObject);

                    _walkerController.gravity = _gravityFly;

                    character.GetModule<SwimModule>().BlockJumpIntoWater();
            
                    onFly.Dispatch();
                }
                else
                {
                    _isFlying = false;
                }
            }, false).SetLink(gameObject);
        }
        
        public void SetNotFly()
        {
            if (!_isFlying) return;
            _isFlying = false;
           
            _notFly.Apply();
            
            if (_flyEffectTween != null)
            {
                _flyEffectTween.Kill();
            }
            if (_parachuteTween != null)
            {
                _parachuteTween.Kill();
            }
            _parachuteTween = _parachute.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _parachute.gameObject.SetActive(false);
            }).SetLink(gameObject);
            
            DOVirtual.DelayedCall(0.1f, () =>
            {
                character.GetModule<SwimModule>().UnblockJumpIntoWater();
            });
            
            _walkerController.gravity = _defaultGravity;
            
            
            onNotFly.Dispatch();
        }

        private void FixedUpdate()
        {
            if(!_isAvailable) return;
            
            if (!_isFlying && CheckForStartFly())
            {
                SetFly();
            }
            else if(isFlying && CheckForStopFly())
            {
                SetNotFly();
            }
        }
        
        private bool CheckForStartFly()
        {
            Ray ray = new Ray(_pointForRaycast.position, Vector3.up * -5f);
            RaycastHit hit;
            
            int layerMask = LayerMask.GetMask("GroundCollider", "SeaCollider");

            if (Physics.Raycast(ray, out hit, 15, layerMask))
            {
                if (hit.distance >= _distanceStartFly)
                {
                    return true;
                }
            }

            return false;
        }
        
        private bool CheckForStopFly()
        {
            Ray ray = new Ray(_pointForRaycast.position, Vector3.up * -5f);
            RaycastHit hit;
            
            int layerMask = LayerMask.GetMask("GroundCollider", "SeaCollider");

            if (Physics.Raycast(ray, out hit, _distanceEndFly + 2, layerMask))
            {
                if (hit.distance <= _distanceEndFly)
                {
                    return true;
                }
            }

            return false;
        }
    }
}