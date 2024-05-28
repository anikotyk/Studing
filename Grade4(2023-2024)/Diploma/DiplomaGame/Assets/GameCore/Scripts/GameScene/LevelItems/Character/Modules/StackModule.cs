using DG.Tweening;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Character.Modules
{
    public class StackModule : InteractorCharacterModule
    {
        [SerializeField] private AnimatorParameterApplier _carryAnimatorApplier;
        [SerializeField] private ProductsCarrier _carrier;
        public ProductsCarrier carrier => _carrier;
        [SerializeField] private Transform _carryContainer;
        [SerializeField] private Transform _containerDefaultPoint;
        [SerializeField] private Transform _containerSwimPoint;
        [SerializeField] private Transform _containerSwimIdlePoint;
        [SerializeField] private Transform _containerAtBackPoint;

        [SerializeField] private bool _isStackVisible;
        public bool isStackVisible => _isStackVisible;

        [SerializeField] private float _moveContainerTime = 0.35f;

        private bool _isSwimIdle = false;
        
        private Sequence _containerMoveSequence;

        public override void Construct()
        {
            base.Construct();

            if (character.GetModule<SwimModule>())
            {
                character.GetModule<SwimModule>().onSwim.On(OnSwim);
                character.GetModule<SwimModule>().onNotSwim.On(OnNotSwim);
            }

            if (character.GetModule<CuttingModule>())
            {
                character.GetModule<CuttingModule>().onCutting.On(OnHittingTree);
                character.GetModule<CuttingModule>().onEndCutting.On(OnNotHittingTree);
            }
            
            if (character.GetModule<HittingCharacterModule>())
            {
                character.GetModule<HittingCharacterModule>().onHitting.On(OnHittingTree);
                character.GetModule<HittingCharacterModule>().onNotHitting.On(OnNotHittingTree);
            }
            
            character.GetModule<CharacterMovingModule>().onStartMoving.On(OnStartMove);
            character.GetModule<CharacterMovingModule>().onStopMoving.On(OnStopMove);

            if (!_isStackVisible)
            {
               // HideStack(false);
            }
        }

        private void OnSwim()
        {
            if (_isSwimIdle)
            {
                JumpStack(_containerSwimPoint);
                EnableHandsCarry();
            }
            else
            {
                MoveStack(_containerSwimPoint);
            }
            _isSwimIdle = false; 
            
        }

        private void OnStartMove()
        {
            if (character.GetModule<SwimModule>() && character.GetModule<SwimModule>().isSwim)
            {
                OnSwim();
            }
        }
        
        private void OnStopMove()
        {
            if (character.GetModule<SwimModule>() && character.GetModule<SwimModule>().isSwim)
            {
                OnSwimIdle();
            }
        }
        
        private void OnSwimIdle()
        {
            JumpStack(_containerAtBackPoint);
            _carryAnimatorApplier.SetLayerWeight(0);
            _isSwimIdle = true;
            //MoveStack(_containerSwimIdlePoint);
        }
        
        private void OnNotSwim()
        {
            if (_isSwimIdle)
            {
                JumpStack(_containerDefaultPoint);
                EnableHandsCarry();
            }
            else
            {
                MoveStack(_containerDefaultPoint);
            }
            
            _isSwimIdle = false;
        }
        
        private void OnHittingTree()
        {
            JumpStack(_containerAtBackPoint);
            _carryAnimatorApplier.SetLayerWeight(0);
        }
        
        private void OnNotHittingTree()
        {
            JumpStack(_containerDefaultPoint);

            EnableHandsCarry();
        }
        
        private void MoveStack(Transform point)
        {
            if(!_isStackVisible) return;
            
            if (_containerMoveSequence != null)
            {
                _containerMoveSequence.Kill();
            }
            _containerMoveSequence = DOTween.Sequence().SetLink(gameObject);
            _carryContainer.SetParent(point);
            _carryContainer.transform.localScale = Vector3.one;
            _containerMoveSequence.Append(_carryContainer.DOLocalMove(Vector3.zero, _moveContainerTime).SetLink(gameObject)).SetLink(gameObject);
            _containerMoveSequence.Join(_carryContainer.DOLocalRotate(Vector3.zero, _moveContainerTime).SetLink(gameObject)).SetLink(gameObject);
        }
        
        private void JumpStack(Transform point)
        {
            if(!_isStackVisible) return;
            
            if (_containerMoveSequence != null)
            {
                _containerMoveSequence.Kill();
            }
            _containerMoveSequence = DOTween.Sequence().SetLink(gameObject);
            _carryContainer.SetParent(point);
            _carryContainer.transform.localScale = Vector3.one;
            _containerMoveSequence.Append(_carryContainer.DOLocalJump(Vector3.zero, 0.02f, 1, _moveContainerTime).SetLink(gameObject)).SetLink(gameObject);
            _containerMoveSequence.Join(_carryContainer.DOLocalRotate(Vector3.zero, _moveContainerTime).SetLink(gameObject)).SetLink(gameObject);
        }

        public void HideStack(bool isToBlockStack = true)
        {
            _carryContainer.gameObject.SetActive(false);
            _carryAnimatorApplier.SetLayerWeight(0);

            if (isToBlockStack)
            {
                BlockStacking();
            }
        }
        
        public void ShowStack(bool isToUnblockStack = true)
        {
            if (isToUnblockStack)
            {
                UnblockStacking();
            }
            _carryContainer.gameObject.SetActive(true);
            EnableHandsCarry();
        }

        public void BlockStacking()
        {
            _carrier.enabled = false;
        }
        
        public void UnblockStacking()
        {
            _carrier.enabled = true;
        }

        private void EnableHandsCarry()
        {
            if(!_isStackVisible) return;
            if(!_carrier.enabled) return;
            
            if (!_carrier.IsEmpty())
            {
                _carryAnimatorApplier.SetLayerWeight(1);
            }
        }
    }
}