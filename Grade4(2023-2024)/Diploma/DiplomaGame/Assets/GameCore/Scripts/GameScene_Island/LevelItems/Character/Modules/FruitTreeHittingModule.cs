using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.GameScene.Helper;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Character.Modules
{
    public class FruitTreeHittingModule : InteractorCharacterModule
    {
        [SerializeField] private AnimatorParameterApplier _hittingAnim;
        public AnimatorParameterApplier hittingAnim => _hittingAnim;
        [SerializeField] private AnimatorParameterApplier _stopHittingAnim;
        public AnimatorParameterApplier stopHittingAnim => _stopHittingAnim;
        protected Coroutine hittingCoroutine;
        
        protected virtual float speedHitting => 6f;
        
        public readonly TheSignal<float> onMove = new();

        private bool _isRunning;
        public bool isRunning => _isRunning;
        public override bool isNowRunning => isRunning;
        
        public InteractionCharacterModule interactionModule => character.GetModule<InteractionCharacterModule>();

        public void PreStartHitting()
        {
            interactionModule.OnInteractionStart(this);
        }
        
        public virtual void StartHitting(FruitTreeItem fruitTreeItem)
        {
            fruitTreeItem.onEndHitting.Once(() =>
            {
                EndHitting();
            });
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
            hittingAnim.Apply();
            
            OnHittingStart(fruitTreeItem);
        }

        public bool CanInteract()
        {
            return interactionModule.CanInteract();
        }
        
        protected virtual void OnHittingStart(FruitTreeItem fruitTreeItem)
        {
            hittingCoroutine = StartCoroutine(HittingCoroutine());
        }

        protected virtual IEnumerator HittingCoroutine()
        {
            float moveCoeff = 0;
            float direction = 1;
            while (true)
            {
                yield return null;
               
                moveCoeff += Time.deltaTime * direction * speedHitting;
                onMove.Dispatch(moveCoeff);
                
                if (moveCoeff >= 1 || moveCoeff <= -1)
                {
                    moveCoeff = direction;
                    direction *= -1;
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        
        protected virtual void EndHitting()
        {
            if (hittingCoroutine != null)
            {
                StopCoroutine(hittingCoroutine);
            }
            
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
            stopHittingAnim.Apply();
            _isRunning = false;
            
            interactionModule.OnInteractionEnd(this);
        }
    }
}