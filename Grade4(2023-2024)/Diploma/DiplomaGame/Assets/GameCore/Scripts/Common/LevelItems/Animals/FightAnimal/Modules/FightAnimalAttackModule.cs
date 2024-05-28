using DG.Tweening;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.Common.Misc;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Sensors;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Animals.FightAnimal.Modules
{
    public class FightAnimalAttackModule : CoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public VfxStack vfxStack { get; }
        [SerializeField] private float _delayAfterHitAnim = 0.5f;
        [SerializeField] private float _delayStartAttack = 1f;
        [SerializeField] private float _delayBetweenAttacks = 3f;
        [SerializeField] private Collider _collider;
        [SerializeField] private AnimatorParameterApplier _hitAnim;
        [SerializeField] private Transform _vfxHitPoint;
        
        private FightAnimalView _viewCached;
        public FightAnimalView view => _viewCached ??= GetComponentInParent<FightAnimalView>(true);

        private InteractorCharacterModel _currentCharacterTarget;
        private bool _isAttacking = false;
        private Tween _attackTween = null;
        private Tween _attackTweenLoop = null;
        private Tween _lookTween = null;
        
        public void OnEnterCharacter(InteractorCharacterModel character)
        {
            if (!_isAttacking)
            {
                StartAttack(character);
            }
        }
        
        public void OnExitCharacter(InteractorCharacterModel character)
        {
            if (_isAttacking && character == _currentCharacterTarget)
            {
                EndAttack();
            }   
        }

        private void StartAttack(InteractorCharacterModel character)
        {
            _isAttacking = true;
            _currentCharacterTarget = character;
           
            view.taskModule.aiPath.canMove = false;
            view.locomotionMovingModule.StopMovement();
            
            _lookTween = DOVirtual.DelayedCall(10f, () => { }, false).OnUpdate(() =>
            {
                Vector3 targetPosition = character.view.transform.position;
                targetPosition.y = view.transform.position.y;
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - view.transform.position);
                view.transform.rotation = Quaternion.Slerp(view.transform.rotation, targetRotation, 0.5f * Time.deltaTime);
            }).SetLoops(-1, LoopType.Incremental).SetLink(gameObject);
            
            _attackTween = DOVirtual.DelayedCall(_delayStartAttack, () =>
            {
                Attack(character);
                _attackTweenLoop = DOVirtual.DelayedCall(_delayBetweenAttacks, () =>
                {
                    if (!IsCollidesWithCharacter(character))
                    {
                        EndAttack();
                        return;
                    }
                    Attack(character);
                }, false).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
            }, false).SetLink(gameObject);
        }

        private void Attack(InteractorCharacterModel character)
        {
            _hitAnim.Apply();
            DOVirtual.DelayedCall(_delayAfterHitAnim, () =>
            {
                vfxStack.Spawn(CommStr.AnimalHitVFX, _vfxHitPoint.position);
             
                if (IsCollidesWithCharacter(character))
                {
                    character.view.GetModule<GetDamageCharacterModule>().GetDamage();
                }
            }, false).SetLink(gameObject);
        }

        private bool IsCollidesWithCharacter(InteractorCharacterModel character)
        {
           return _collider.bounds.Intersects(character.view.GetModule<SurroundInteractorCharacterSensorModule>().sensor
               .bounds);
        }
        
        public void EndAttack()
        {
            if (_lookTween != null)
            {
                _lookTween.Kill();
            }
            if (_attackTween != null)
            {
                _attackTween.Kill();
            }
            if (_attackTweenLoop != null)
            {
                _attackTweenLoop.Kill();
            }
            
            view.taskModule.aiPath.canMove = true;
            view.locomotionMovingModule.StartMovement();
            _currentCharacterTarget = null;
            _isAttacking = false;
        }
    }
}