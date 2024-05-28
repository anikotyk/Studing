using System.Collections;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.GameScene_Island.Audio;
using GameCore.GameScene.Audio;
using GameCore.GameScene.LevelItems.Character.Modules;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Character.Modules
{
    public class AnimalInteractModule : InteractorCharacterModule
    {
        [InjectOptional, UsedImplicitly] public MainCharacterWalkerCoreSpeedController walkerSpeedController { get; }
        [Inject, UsedImplicitly] public CutSoundManager cutSoundManager { get; }
        
        [SerializeField] private AnimatorParameterApplier _cuttingAnim;
        [SerializeField] private AnimatorParameterApplier _stopCuttingAnim;
        [SerializeField] private AnimatorParameterApplier _cowAnim;
        [SerializeField] private AnimatorParameterApplier _stopCowAnim;
        [SerializeField] private GameObject _scissors;

        private Coroutine _cuttingSoundCoroutine = null;
        
        private bool _isRunning;
        public bool isRunning => _isRunning;
        public override bool isNowRunning => isRunning;
        
        public InteractionCharacterModule interactionModule => character.GetModule<InteractionCharacterModule>();

        public bool CanInteract()
        {
            return interactionModule.CanInteract();
        }
        
        public void OnStartInteract()
        {
            _isRunning = true;
            DisableMovement();
            character.GetModule<StackModule>().HideStack(false);
            
            interactionModule.OnInteractionStart(this);
        }
        
        public void OnSheepInteract()
        {
            _cuttingAnim.Apply();
            _scissors.gameObject.SetActive(true);
            
            if (character is MainCharacterView)
            {
                _cuttingSoundCoroutine = StartCoroutine(PlaySoundLoop(cutSoundManager, 0.5f));
            }
        }

        private IEnumerator PlaySoundLoop(PoolSoundManager audioManager, float interval)
        {
            while (true)
            {
                audioManager.PlaySound();
                yield return new WaitForSeconds(interval);
            }
        }
        
        public void OnCowInteract()
        {
            _cowAnim.Apply();
        }
        
        public void OnEndInteract()
        {
            _stopCuttingAnim.Apply();
            _stopCowAnim.Apply();
            if (_cuttingSoundCoroutine != null)
            {
                StopCoroutine(_cuttingSoundCoroutine);
            }
            _scissors.gameObject.SetActive(false);
            EnableMovement();
            character.GetModule<StackModule>().ShowStack();

            _isRunning = false;
            
            interactionModule.OnInteractionEnd(this);
        }

        protected virtual void DisableMovement()
        {
            walkerSpeedController.SetMaxSpeed(0);
        }
        
        protected virtual void EnableMovement()
        {
            walkerSpeedController.ResetMaxSpeed();
        }
    }
}