using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.Helper;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Sensors;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Character.Modules
{
    public class MillGrindingModule : InteractorCharacterModule
    {
        [SerializeField] private AnimatorParameterApplier _walkAnim;
        public AnimatorParameterApplier walkAnim => _walkAnim;
        [SerializeField] private AnimatorParameterApplier _stopWalkAnim;
        public AnimatorParameterApplier stopWalkAnim => _stopWalkAnim;
        [SerializeField] private AnimatorParameterApplier _handsHoldAnim;
        [SerializeField] private AnimatorParameterApplier _stopHandsHoldAnim;
        
        protected Coroutine grindingCoroutine;
        
        protected virtual float speedGriding => 0.15f;

        public readonly TheSignal<float> onMove = new();

        private bool _isRunning;
        public bool isRunning => _isRunning;
        public override bool isNowRunning => isRunning;
        
        public InteractionCharacterModule interactionModule => character.GetModule<InteractionCharacterModule>();

        public void StartGrinding(MillItem millItem)
        {
            interactionModule.OnInteractionStart(this);
            character.GetModule<SurroundInteractorCharacterSensorModule>().sensor.enabled = false;
            
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
                character.GetModule<StackModule>().HideStack();
            },false).SetLink(gameObject);

            _isRunning = true;
            
            _handsHoldAnim.Apply();
            
            OnStartGrinding(millItem);
        }

        public bool CanInteract()
        {
            return interactionModule.CanInteract();
        }

        protected virtual void OnStartGrinding(MillItem millItem)
        { 
            walkAnim.Apply();
            grindingCoroutine = StartCoroutine(GrindingCoroutine());
        }

        protected virtual IEnumerator GrindingCoroutine()
        {
            float moveCoeff = 0;
            
            while (true)
            {
                yield return null;
                moveCoeff += Time.deltaTime * speedGriding;
                onMove.Dispatch(moveCoeff);
            }
        }
        
        public void EndGrinding()
        {
            stopWalkAnim.Apply();
            _stopHandsHoldAnim.Apply();
            StopCoroutine(grindingCoroutine);
            OnEndGrinding();
           
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
            
            character.GetModule<StackModule>().ShowStack();

            _isRunning = false;
            
            character.GetModule<SurroundInteractorCharacterSensorModule>().sensor.enabled = true;
            interactionModule.OnInteractionEnd(this);
        }
        
        protected virtual void OnEndGrinding()
        {
            
        }
    }
}