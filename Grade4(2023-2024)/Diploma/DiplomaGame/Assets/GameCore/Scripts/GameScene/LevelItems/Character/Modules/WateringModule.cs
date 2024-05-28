using DG.Tweening;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.GameScene.Helper;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Settings;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Character.Modules
{
    public class WateringModule : InteractorCharacterModule
    {
        [SerializeField] protected AnimatorParameterApplier _runWatering;
        public AnimatorParameterApplier runWatering => _runWatering;
        [SerializeField] protected AnimatorParameterApplier _stopRunWatering;
        public AnimatorParameterApplier stopRunWatering => _stopRunWatering;
        private Tween _wateringTween;
        
        public readonly TheSignal onEndedWatering = new();
        public readonly TheSignal onCancelledWatering = new();
        
        public readonly TheSignal onStartRun = new();
        public readonly TheSignal onStopRun = new();

        private bool _isRunning;
        public bool isRunning => _isRunning;
        public override bool isNowRunning => isRunning;
        
        public InteractionCharacterModule interactionModule => character.GetModule<InteractionCharacterModule>();

        public virtual void StartWatering(WaterFilterObject waterFilterObject)
        {
            interactionModule.OnInteractionStart(this);
            
            if (character.TryGetComponent<MainCharacterSimpleWalkerController>(
                    out MainCharacterSimpleWalkerController mover))
            {
                mover.TurnOffMovement();
            }

            if (character.TryGetComponent<HelperAIPath>(
                    out HelperAIPath helperAIPath))
            {
                helperAIPath.canMove = false;
            }
            
            DOVirtual.DelayedCall(0.05f, () =>
            {
                if(character.GetModule<StackModule>()) character.GetModule<StackModule>().HideStack();
            },false).SetLink(gameObject);

            _isRunning = true;

            OnStartWatering();
        }

        public bool CanInteract()
        {
            return interactionModule.CanInteract();
        }

        protected virtual void OnStartWatering()
        {
            _runWatering.Apply();
            onStartRun.Dispatch();
            
            _wateringTween = DOVirtual.DelayedCall(GameplaySettings.def.wateringData.wateringTime, () =>
            {
                EndWatering();
            },false).SetLink(gameObject);
        }
        
        public virtual void EndWatering()
        {
            _stopRunWatering.Apply();
            onStopRun.Dispatch();
            
            if (character.TryGetComponent<MainCharacterSimpleWalkerController>(
                    out MainCharacterSimpleWalkerController mover))
            {
                mover.TurnOnMovement();
            }
            
            if (character.TryGetComponent<HelperAIPath>(
                    out HelperAIPath helperAIPath))
            {
                helperAIPath.canMove = true;
            }
            
            if(character.GetModule<StackModule>()) character.GetModule<StackModule>().ShowStack();

            _isRunning = false;
            
            interactionModule.OnInteractionEnd(this);
            
            onEndedWatering.Dispatch();
        }

        public void StopWateringInternal()
        {
            if(!_isRunning) return;
            
            if (_wateringTween != null)
            {
                _wateringTween.Kill();
            }
            
            _stopRunWatering.Apply();
            if (character.TryGetComponent<HelperAIPath>(
                    out HelperAIPath helperAIPath))
            {
                if (character is HelperView && (character as HelperView).logicModule && (character as HelperView).logicModule.isTurnedOn)
                {
                    helperAIPath.canMove = true;
                }
            }
            
            _isRunning = false;
        }

        public virtual void CancelWatering()
        {
            _stopRunWatering.Apply();
            onStopRun.Dispatch();
            
            if (character.TryGetComponent<MainCharacterSimpleWalkerController>(
                    out MainCharacterSimpleWalkerController mover))
            {
                mover.TurnOnMovement();
            }
            
            if (character.TryGetComponent<HelperAIPath>(
                    out HelperAIPath helperAIPath))
            {
                helperAIPath.canMove = true;
            }
            
            if(character.GetModule<StackModule>()) character.GetModule<StackModule>().ShowStack();

            _isRunning = false;
            
            interactionModule.OnInteractionEnd(this);
            
            onCancelledWatering.Dispatch();
        }
    }
}