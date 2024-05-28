using DG.Tweening;
using GameBasicsCore.Game.Misc;
using StaserSDK.Extentions;
using UnityEngine;

namespace GameCore.CutScenes.Animations
{
    public class CutSceneJumpAnimationItem : CutSceneAnimationItem
    {
        [SerializeField] private BoatCutScene _boatCutScene;
        [SerializeField] private AnimatorParameterApplier _jumpParameter;
        [SerializeField] private Transform _jumpDestination;
        [SerializeField] private float _jumpDeltaRange;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpDuration;
        [SerializeField] private float _lookAtDuration;
        
        protected override void PrepareInternal()
        {
            foreach (var character in _boatCutScene.characters)
            {
                _jumpParameter.SetAnimator(character.animator);
                _jumpParameter.Apply();
            }
        }

        protected override Tween AnimateInternal()
        {
            var sequence = DOTween.Sequence();
            float jumpOffset = 0;
            foreach (var character in _boatCutScene.characters)
            {
                Vector3 destination = _jumpDestination.position + (jumpOffset * Vector2.right).XZ();
                sequence.Join(character.model.transform.DOJumpY(destination, _jumpHeight, _jumpDuration)).SetLink(gameObject);
                sequence.Join(character.model.transform.DOLookAt(destination, _lookAtDuration));
                jumpOffset += _jumpDeltaRange;
            }

            return sequence;
        }
    }
}