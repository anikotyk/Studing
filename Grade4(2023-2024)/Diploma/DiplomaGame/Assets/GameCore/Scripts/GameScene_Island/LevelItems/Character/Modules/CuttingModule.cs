using DG.Tweening;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.Common.LevelItems.Items;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Character.Modules
{
    public class CuttingModule : InteractorCharacterModule
    {
        [InjectOptional, UsedImplicitly] public MainCharacterWalkerCoreSpeedController walkerSpeedController { get; }
        
        [SerializeField] private AnimatorParameterApplier _scythingAnim;
        [SerializeField] private AnimatorParameterApplier _stopScythingAnim;
        [SerializeField] private float _moveSpeed;
        public float moveSpeed => _moveSpeed;
        [SerializeField] private GameObject _scytheGO;
        [SerializeField] private Scythe _scythe;
        [SerializeField] private ProductsGroupDataConfig _acceptableGroupDataConfigs;
        
        private bool _isRunning;
        public bool isRunning => _isRunning;
        public override bool isNowRunning => isRunning;

        private int _startCuttingCnt;
        
        private Tween _showObjTween;
        private Tween _turnOnInteractTween;
        
        public InteractionCharacterModule interactionModule => character.GetModule<InteractionCharacterModule>();

        public readonly TheSignal onCutting = new();
        public readonly TheSignal onEndCutting = new();

        public override void Construct()
        {
            base.Construct();
            character.GetModule<LocomotionCharacterMovingModule>().onStartMoving.On(() =>
            {
                EndCuttingFully();
            });
        }

        public void StartCutting()
        {
            _startCuttingCnt++;
            if(_isRunning) return;
            
            interactionModule.OnInteractionStart(this);
            
            _scythingAnim.Apply();
            _isRunning = true;
            
            if (_showObjTween != null)
            {
                _showObjTween.Kill();
            }
            if (_turnOnInteractTween != null)
            {
                _turnOnInteractTween.Kill();
            }
            _showObjTween = DOVirtual.DelayedCall(0.15f, () =>
            {
                _scytheGO.gameObject.SetActive(true);
                
                _turnOnInteractTween = DOVirtual.DelayedCall(0.05f, () =>
                {
                    _scythe.TurnOn();
                }, false).SetLink(gameObject);
            }, false).SetLink(gameObject);
            
            SetSpeedCutting();

            onCutting.Dispatch();
        }

        protected virtual void SetSpeedCutting()
        {
            walkerSpeedController.SetMaxSpeed(_moveSpeed);
        }

        public void EndCutting()
        {
            if(!_isRunning) return;
            
            _startCuttingCnt--;
            if(_startCuttingCnt > 0) return;
            _startCuttingCnt = 0;
            
            _stopScythingAnim.Apply();
            _isRunning = false;
            
            if (_showObjTween != null)
            {
                _showObjTween.Kill();
            }
            _scytheGO.gameObject.SetActive(false);
            _scythe.TurnOff();
            
            SetSpeedEndCutting();
            
            interactionModule.OnInteractionEnd(this);
            
            onEndCutting.Dispatch();
        }

        public void EndCuttingFully()
        {
            _startCuttingCnt = 0;
            EndCutting();
        }
        
        protected virtual void SetSpeedEndCutting()
        {
            walkerSpeedController.ResetMaxSpeed();
        }

        public bool IsAbleToCut(ProductDataConfig product)
        {
            return _acceptableGroupDataConfigs.Contains(product);
        }

        public bool CanCutNow(ProductDataConfig product)
        {
            return IsAbleToCut(product) && isRunning;
        }

        public bool CanInteract()
        {
            return interactionModule.CanInteract() || isRunning;
        }
    }
}