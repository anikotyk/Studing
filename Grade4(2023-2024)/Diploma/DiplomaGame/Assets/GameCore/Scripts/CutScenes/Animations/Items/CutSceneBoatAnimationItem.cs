using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Dreamteck.Splines;
using GameCore.ShipScene.Extentions;
using UnityEngine;

namespace GameCore.CutScenes.Animations
{
    public class CutSceneBoatAnimationItem : CutSceneAnimationItem
    {
        [SerializeField] private BoatCutScene _boatCutScene;
        [SerializeField] private SplinePositioner _splinePositioner;
        [SerializeField, Range(0, 1)] private float _startPosition;
        [SerializeField, Range(0, 1)] private float _endPosition;
        [SerializeField] private float _moveDuration;
        [SerializeField] private Ease _moveEase;
        [SerializeField] private float _delay;
        [SerializeField] private List<CharacterPrepareData> _characterPrepareDatas;
        [SerializeField] private CinemachineVirtualCamera _targetCamera;
        
        [System.Serializable]
        public class CharacterPrepareData
        {
            public BoatCutSceneCharacter character;
            public ReversibleAnimatorApplier animation;
            public bool enablePaddle;
        }
        
        protected override void PrepareInternal()
        {
            _targetCamera.gameObject.SetActive(true);
            foreach (var characterPrepareData in _characterPrepareDatas)
            {
                if(characterPrepareData.enablePaddle)
                    characterPrepareData.character.paddle.SetActive(true);
                characterPrepareData.animation.SetAnimator(characterPrepareData.character.animator);
                characterPrepareData.animation.Apply();
            }

            _splinePositioner.SetPercent(_startPosition);
            _boatCutScene.MoveCharactersToDefault();
        }

        protected override Tween AnimateInternal()
        {
            DOVirtual.DelayedCall(_moveDuration + _delay, OnComplete).SetUpdate(false);
            return DOVirtual.Float(_startPosition, _endPosition, _moveDuration, 
                    value => _splinePositioner.SetPercent(value))
                .SetEase(_moveEase).SetDelay(_delay).SetLink(gameObject);
        }

        private void OnComplete()
        {
            RevertAnimations();
        }
        private void RevertAnimations()
        {
            foreach (var characterPrepareData in _characterPrepareDatas)
            {
                if(characterPrepareData.enablePaddle)
                    characterPrepareData.character.paddle.SetActive(false);
                characterPrepareData.animation.SetAnimator(characterPrepareData.character.animator);
                characterPrepareData.animation.Revert();
            }
        }
    }
}