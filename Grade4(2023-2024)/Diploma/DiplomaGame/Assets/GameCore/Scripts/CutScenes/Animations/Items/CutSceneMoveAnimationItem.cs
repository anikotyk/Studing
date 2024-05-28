using Cinemachine;
using DG.Tweening;
using GameCore.ShipScene.Extentions;
using GameBasicsSignals;
using StaserSDK.Extentions;
using UnityEngine;

namespace GameCore.CutScenes.Animations.Items
{
    public class CutSceneMoveAnimationItem : CutSceneAnimationItem
    {
        [SerializeField] private BoatCutScene _boatCutScene;
        [SerializeField] private Transform _target;
        [SerializeField] private ReversibleAnimatorApplier _speedParameter;
        [SerializeField] private float _destinationRange;
        [SerializeField] private float _speed;
        [SerializeField] private float _lookAtDuration;
        [SerializeField, Range(0, 1)] private float _minSpeed;
        [SerializeField] private float _runSpeedDownDuration;
        [Header("Cameras")]
        [SerializeField] private CinemachineVirtualCamera _targetCamera;
        [SerializeField] private CinemachineVirtualCamera _disableCamera;
        
        public readonly TheSignal onCutsceneCompleted = new();
        
        protected override void PrepareInternal()
        {
            _targetCamera.gameObject.SetActive(true);
            _disableCamera.gameObject.SetActive(false);
        }

        protected override Tween AnimateInternal()
        {
            var sequence = DOTween.Sequence();
            Vector3 targetPosition = _target.position;
            float moveOffset = 0;
            foreach (var character in _boatCutScene.characters)
            {
                ApplySpeedParameter(character);
                
                float distance = Vector3.Distance(character.model.transform.position, targetPosition);
                float duration = distance / _speed;
                Vector3 destination = targetPosition + (moveOffset * Vector2.right).XZ();

                sequence.Join(ModeToDestination(character, duration, destination));
                sequence.Join(SpeedDown(character, duration));
                sequence.Join(character.model.transform.DOLookAt(destination, _lookAtDuration));

                moveOffset += _destinationRange;
            }

            return sequence;
        }

        private void ApplySpeedParameter(BoatCutSceneCharacter character)
        {
            _speedParameter.SetAnimator(character.animator);
            _speedParameter.Apply();
        }
        
        private Tween SpeedDown(BoatCutSceneCharacter character, float duration)
        {
            return DOVirtual.DelayedCall(duration - _runSpeedDownDuration, () =>
            {
                DOVirtual.Float(_speedParameter.defaultFloat, _minSpeed, _runSpeedDownDuration, value =>
                {
                    _speedParameter.SetAnimator(character.animator);
                    _speedParameter.ApplyAsFloat(value);
                }).SetId(character);
            });
        }

        private Tween ModeToDestination(BoatCutSceneCharacter character, float duration, Vector3 destination)
        {
            return character.model.transform.DOMove(destination, duration).OnComplete(() =>
            {
                DOTween.Kill(character);
                _speedParameter.SetAnimator(character.animator);
                _speedParameter.Revert();
            }).SetEase(Ease.Linear).OnComplete(()=>
            {
                _targetCamera.gameObject.SetActive(false);
                onCutsceneCompleted.Dispatch();
            });
        }
    }
}