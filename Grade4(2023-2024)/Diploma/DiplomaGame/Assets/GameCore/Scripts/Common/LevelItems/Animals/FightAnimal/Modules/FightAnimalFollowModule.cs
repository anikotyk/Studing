using DG.Tweening;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.Common.LevelItems.Animals.FightAnimal.Modules
{
    public class FightAnimalFollowModule : CoreMonoBehaviour
    {
        [SerializeField] private float _distanceSlowdown;
        private FightAnimalView _viewCached;
        public FightAnimalView view => _viewCached ??= GetComponentInParent<FightAnimalView>(true);

        private InteractorCharacterModel _currentCharacterTarget;
        private Tween _startRandomWalkTween;
        private Tween _stopFollowTween;
        private Tween _slowDownTween;
        private bool _isFollowing;
        
        public void OnEnterCharacter(InteractorCharacterModel character)
        {
            if (!_isFollowing)
            {
                StartFollow(character);
            }
        }
        
        public void OnExitCharacter(InteractorCharacterModel character)
        {
            if (_isFollowing && character == _currentCharacterTarget)
            {
                _isFollowing = false;   
                _stopFollowTween = DOVirtual.DelayedCall(0.5f, ()=>
                {
                    EndFollow(true);
                }, false).SetLink(gameObject);
            }   
        }

        private void StartFollow(InteractorCharacterModel character)
        {
            if (_stopFollowTween!=null) _stopFollowTween.Kill();
            if (_startRandomWalkTween != null) _startRandomWalkTween.Kill();
            if (_slowDownTween!=null) _slowDownTween.Kill();
            
            _isFollowing = true;
            _currentCharacterTarget = character;
            SpeedUp();
            view.walkRandomModule.StartWalkRandomly();
            view.taskModule.aiPath.SetTarget(character.view.transform);

            _slowDownTween = DOVirtual.DelayedCall(10f, (() => { }), false).OnUpdate(() =>
            {
                if (Vector3.Distance(view.transform.position, character.view.transform.position) <= _distanceSlowdown)
                {
                    SlowDown();
                }
                else
                {
                    SpeedUp();
                }
            }).SetLoops(-1, LoopType.Incremental).SetLink(gameObject);
        }

        private void SlowDown()
        {
            view.speedModule.ResetSpeed();
        }
        
        private void SpeedUp()
        {
            view.speedModule.SetSpeed(view.speedModule.maxSpeed);
        }

        public void EndFollow(bool isToStartWalkRandomly = true)
        {
            if(_slowDownTween!=null) _slowDownTween.Kill();
            if(_stopFollowTween!=null) _stopFollowTween.Kill();
            _currentCharacterTarget = null;
            _isFollowing = false;
            SlowDown();
            view.taskModule.aiPath.RemoveTargetPoint();
            view.locomotionMovingModule.StopMovement();
            view.taskModule.aiPath.SetDestination(view.transform.position);
            if (_startRandomWalkTween != null)
            {
                _startRandomWalkTween.Kill();
            }
            if (isToStartWalkRandomly)
            {
                _startRandomWalkTween = DOVirtual.DelayedCall(1f, () =>
                {
                    view.walkRandomModule.StartWalkRandomly();
                }).SetLink(gameObject);
            }
        }
    }
}