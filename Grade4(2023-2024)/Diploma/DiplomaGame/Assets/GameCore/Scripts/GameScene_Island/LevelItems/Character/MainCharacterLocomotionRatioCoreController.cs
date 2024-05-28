using System;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Character
{
    public class MainCharacterLocomotionRatioCoreController : ControllerInternal, ILateTickable
    {
        [Inject, UsedImplicitly] public MainCharacterView mainCharacter { get; }
        [Inject, UsedImplicitly] public MainCharacterSimpleWalkerController simpleWalkerController { get; }
        [Inject, UsedImplicitly] public MainCharacterWalkerCoreSpeedController walkerSpeedController { get; }

        private float _prev = -1;
        
        private LocomotionCharacterMovingModule _locomotionMovingModule;

        public override void Construct()
        {
            base.Construct();
            _locomotionMovingModule = mainCharacter.GetModule<LocomotionCharacterMovingModule>();
        }

        public void LateTick()
        {
            var ratio = (_locomotionMovingModule.isMoving 
                    ? (float)Math.Round(mainCharacter.rigidbody.velocity.magnitude / simpleWalkerController.movementSpeed, 2) : 0)
                .MaxClamp(walkerSpeedController.maxRatio);
            if (Math.Abs(ratio - _prev) > 0.01f)
            {
                _prev = ratio;
                _locomotionMovingModule.SetRatio(ratio);
            }
        }
    }
}