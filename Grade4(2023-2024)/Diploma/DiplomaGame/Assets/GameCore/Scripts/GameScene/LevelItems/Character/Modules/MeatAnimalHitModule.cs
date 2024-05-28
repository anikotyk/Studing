using System.Collections;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Settings;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Sensors;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Character.Modules
{
    public class MeatAnimalHitModule : InteractorCharacterModule
    {
        [SerializeField] private GameObject _spear;
        [SerializeField] private AnimatorParameterApplier _hitApplier;
        [SerializeField] private AnimatorParameterApplier _hitEndApplier;
        [SerializeField] private ParticleSystem _vfx;
        [SerializeField] private AudioSource _sound;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _correctDistanceShark = 1.5f;
        [SerializeField] private float _delayBetweetAnim = 0.4f;
        [SerializeField] private float _delayHitAfterAnim = 0.33f;
        
        private Coroutine _hitCoroutine;
        
        private bool _isHitting;
        public bool isHitting => _isHitting;
        public override bool isNowRunning => isHitting;

        private bool _isBlocked = false;

        private MeatAnimalItem _currentMeatAnimal;
        
        public float angleCanHit => GameplaySettings.def.sharksData.angleCanHit;
        
        public InteractionCharacterModule interactionModule => character.GetModule<InteractionCharacterModule>();
        
        public readonly TheSignal onHitting = new();
        public readonly TheSignal onNotHitting = new();

        public override void Construct()
        {
            base.Construct();

            character.hub.Get<GCSgnl.SharkSignals.Interact>().On((interactor, item) =>
            {
                if (interactor.view == interactorCharacter)
                {
                    EnteredShark(item);
                }
            });

            character.GetModule<CharacterMovingModule>().onStartMoving.On(() =>
            {
                ExitedCurrentShark();
            });
        }

        public void EnteredShark(MeatAnimalItem item)
        {
            if(_isBlocked) return;
            _currentMeatAnimal = item;

            if (!_isHitting)
            {
                StartHitting();
            }
        }

        private void ExitedCurrentShark()
        {
            if (_currentMeatAnimal != null)
            {
                ExitedShark(_currentMeatAnimal);
            }
        }

        public void ExitedShark(MeatAnimalItem item)
        {
            _currentMeatAnimal = null;
            StopHitting();
        }

        public void StopHitting()
        {
            if (!_isHitting) return;
            _isHitting = false;
            
            if (_hitCoroutine != null)
            {
                StopCoroutine(_hitCoroutine);
                _hitCoroutine = null;
            }
            
            _spear.SetActive(false);
            _hitEndApplier.Apply();
            
            interactionModule.OnInteractionEnd(this);
            
            onNotHitting.Dispatch();
        }
        
        public void StartHitting()
        {
            if(!interactionModule.CanInteract()) return;
            if (_isHitting) return;
            _isHitting = true;
            
            interactionModule.OnInteractionStart(this);
            
            _spear.SetActive(true);
            onHitting.Dispatch();
            
            _hitCoroutine = StartCoroutine(HitSharkCoroutine());
        }

        private IEnumerator HitSharkCoroutine()
        {
            while (true)
            {
                if (_currentMeatAnimal.isDead) break;

                if (!IsCorrectDistance(_currentMeatAnimal)) break;
                if (IsCorrectAngle(_currentMeatAnimal) && _currentMeatAnimal.isEnabled)
                {
                   // while (_animator.GetCurrentAnimatorStateInfo(0).IsName("HitShark")) yield return null;

                    _hitApplier.Apply();
                    yield return new WaitForSeconds(_delayHitAfterAnim);
                    _vfx.Play();
                
                    if (_sound != null)
                    {
                        _sound.Play();
                    }

                    if (_currentMeatAnimal && _currentMeatAnimal.isEnabled)
                    {
                        _currentMeatAnimal.OnHit();
                    }
                    
                    if (_currentMeatAnimal.isDead) break;
                    
                    yield return new WaitForSeconds(_delayBetweetAnim);
                }

                yield return null;
            }
            
            StopHitting();
        }
        
        private bool IsCorrectAngle(MeatAnimalItem item)
        {
            Vector3 distance = item.transform.position - character.transform.position;
            return Vector3.Angle(character.transform.forward, distance) <= angleCanHit;
        }
        
        private bool IsCorrectDistance(MeatAnimalItem item)
        {
            Vector3 distance = item.transform.position - character.transform.position;
            return distance.magnitude <= _correctDistanceShark;
        }
    }
}