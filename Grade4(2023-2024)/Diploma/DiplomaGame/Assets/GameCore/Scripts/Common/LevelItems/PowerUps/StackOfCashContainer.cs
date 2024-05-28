using DG.Tweening;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.Common.LevelItems.PowerUps
{
    public class StackOfCashContainer : PowerUpContainer
    {
        [SerializeField] private Animator _animator;
        
        [SerializeField] private AnimatorParameterApplier _openChest;
        [SerializeField] private AnimatorParameterApplier _closeChest;
        [SerializeField] private AnimatorParameterApplier _claimChest;
        
        private Tween _hideTween;
        private float _delayHideAfterOpenObject = 1f;
        
        public override void OnCharacterEntered()
        {
            _openChest.Apply();
            base.OnCharacterEntered();
        }
        
        public override void OnCharacterExited()
        {
            _closeChest.Apply();
            base.OnCharacterExited();
        }
        
        protected override void DeactivateObject()
        {
            _animator.writeDefaultValuesOnDisable = true;
            base.DeactivateObject();
        }
        
        public override void Hide()
        {
            DisableInteractions();
            canvas.gameObject.SetActive(false);
            _hideTween = transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InBack).OnComplete(()=>
            {
                DeactivateObject();
            }).SetLink(gameObject);
        }

        public override void OnClaimed()
        {
            isClaimed = true;
            onClaimed.Dispatch();
            DisableInteractions();
            _claimChest.Apply();
            interactItem.enabled = false;
            DOVirtual.DelayedCall(_delayHideAfterOpenObject, Hide, false).SetLink(gameObject);
        }

        public override void OnCameraAttention()
        {
            _openChest.Apply();
        }
        
        public override void OnCameraAttentionEnded()
        {
            if (!isClaimed)
            {
                _closeChest.Apply();
            }
        }
    }
}
