using GameCore.GameScene.LevelItems.Character.Modules;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsSDK.Modules.IdleArcade.Controllers.CharacterContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Speeds;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Character
{
    public class MainCharacterWalkerCoreSpeedController : ControllerInternal, IInitializable
    {
        [Inject, UsedImplicitly] public InteractorCharacterProductCarriersController productCarriersController { get; }
        [Inject, UsedImplicitly] public MainCharacterSimpleWalkerController simpleWalkerController { get; }
        [Inject, UsedImplicitly] public MainCharacterView mainCharacter { get; }

        private MainCharacterSpeedModule _speedModule;
        private LocomotionCharacterMovingModule _locomotionMovingModule;
        private StackModule _stackModule;

        public float maxRatio;
        
        private float _newMaxSpeed = -1;
        private float _newMaxRatio = -1;

        
        public override void Construct()
        {
            productCarriersController.onChange.On(Validate);
            
            _locomotionMovingModule = mainCharacter.GetModule<LocomotionCharacterMovingModule>();
            _speedModule = mainCharacter.GetModule<MainCharacterSpeedModule>();
            _stackModule = mainCharacter.GetModule<StackModule>();
            _speedModule.onChange.On(Validate);
        }

        public void Initialize()
        {
            Validate();
        }

        private void Validate()
        {
            if (_newMaxSpeed > -1)
            {
                simpleWalkerController.movementSpeed = _newMaxSpeed;
                maxRatio = _newMaxRatio;
                return;
            }
            
            if (productCarriersController.countAllCarryingProducts > 0 && _stackModule.isStackVisible)
            {
                simpleWalkerController.movementSpeed = _speedModule.walkingSpeed;
                maxRatio = _locomotionMovingModule.walkingAnimationRatio;
                return;
            }

            simpleWalkerController.movementSpeed = _speedModule.runningSpeed;
            maxRatio = _locomotionMovingModule.runningAnimationRatio;
        }

        public void SetMaxSpeed(float speed)
        {
            _newMaxSpeed = speed;
            _newMaxRatio = _locomotionMovingModule.runningAnimationRatio * speed / _speedModule.runningSpeed;
            Validate();
        }
        
        public void ResetMaxSpeed()
        {
            _newMaxSpeed = -1;
            _newMaxRatio = -1;
            Validate();
        }
    }
}