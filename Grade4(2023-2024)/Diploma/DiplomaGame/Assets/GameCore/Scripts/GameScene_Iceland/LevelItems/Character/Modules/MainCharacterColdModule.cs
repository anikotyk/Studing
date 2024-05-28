using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Character.Modules;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Character.Modules
{
    public class MainCharacterColdModule : InteractorCharacterModule
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        [SerializeField] private BuyObject _endColdBuyObject;
        [SerializeField] private AnimatorParameterApplier _coldAnim;
        [SerializeField] private AnimatorParameterApplier _endColdAnim;
        [SerializeField] private string _idleParamName = "IdleIndex";
        [SerializeField] private float _delayStartFreezing = 0.5f;
        [SerializeField] private float _animationsTransitionTime = 0.5f;
        [SerializeField] private ParticleSystem _coldVfx;
        [SerializeField] private bool _enableVfx;

        private InteractionCharacterModule _interactionCharacterModule;
        private CharacterMovingModule _characterMovingModule;
        
        private Tween _animTween;
        private bool _isCanBeCold;
        private bool _isInWarmZone;

        public override void Construct()
        {
            base.Construct();

            _interactionCharacterModule = character.GetModule<InteractionCharacterModule>();
            _characterMovingModule = character.GetModule<CharacterMovingModule>();
            
            initializeInOrderController.Add(Initialize, 2000);
        }
        
        private void Initialize()
        {
            if (!_endColdBuyObject.isBought)
            {
                _isCanBeCold = true;
                
                _endColdBuyObject.onBuy.Once(() =>
                {
                    DisableModule();
                });

                _characterMovingModule.onStartMoving.On(HideCold);
                _characterMovingModule.onStopMoving.On(ShowCold);
                _interactionCharacterModule.onInteractionsAvailable.On(ShowCold);
                _interactionCharacterModule.onInteractionsLocked.On(HideCold);

                _coldAnim.Apply();
            }
        }

        private void DisableModule()
        {
            _isCanBeCold = false;
            
            _characterMovingModule.onStartMoving.Off(HideCold);
            _characterMovingModule.onStopMoving.Off(ShowCold);
            _interactionCharacterModule.onInteractionsAvailable.Off(ShowCold);
            _interactionCharacterModule.onInteractionsLocked.Off(HideCold);

            HideCold();
            _endColdAnim.Apply();
        }

        private void ShowCold()
        {
            if (!_isCanBeCold) return;
            _endColdAnim.Apply();
            if(!_interactionCharacterModule.CanInteract() || _characterMovingModule.isMoving) return;
            if (_isInWarmZone) return;

            _animTween = DOVirtual.DelayedCall(_delayStartFreezing, () =>
            {
                if(_animTween!=null) _animTween.Kill();
                float timer = character.animator.GetFloat(_idleParamName) * _animationsTransitionTime;
                _animTween = DOVirtual.DelayedCall(_animationsTransitionTime, () =>
                {
                    if(_enableVfx && _coldVfx) _coldVfx.gameObject.SetActive(true);
                }, false).OnUpdate(() =>
                {
                    float coef = Mathf.Clamp(timer / _animationsTransitionTime, 0, 1);
                    character.animator.SetFloat(_idleParamName, coef);
                    timer += Time.deltaTime;
                }).SetLink(gameObject);
            }, false).SetLink(gameObject);
        }
        
        private void HideCold()
        {
            if(_animTween!=null) _animTween.Kill();
            if(_enableVfx && _coldVfx) _coldVfx.gameObject.SetActive(false);
        }

        public void EnterWarmZone()
        {
            _isInWarmZone = true;
            if (!_isCanBeCold) return;
            if (!_interactionCharacterModule.CanInteract() || _characterMovingModule.isMoving)
            {
                _endColdAnim.Apply();
            }
            else
            {
                if(_animTween!=null) _animTween.Kill();
                float timer = (1 - character.animator.GetFloat(_idleParamName)) * _animationsTransitionTime;
                _animTween = DOVirtual.DelayedCall(_animationsTransitionTime, () =>
                {
                    if(_enableVfx && _coldVfx) _coldVfx.gameObject.SetActive(false);
                }, false).OnUpdate(() =>
                {
                    float coef = Mathf.Clamp(1f - (timer / _animationsTransitionTime), 0, 1);
                    character.animator.SetFloat(_idleParamName, coef);
                    timer += Time.deltaTime;
                }).SetLink(gameObject);
            }
        }
        
        public void ExitWarmZone()
        {
            _isInWarmZone = false;
        }
    }
}