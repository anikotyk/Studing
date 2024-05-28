using System;
using GameCore.GameScene_Iceland.LevelItems.Items;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using Zenject;

namespace GameCore.GameScene_Iceland.Controllers
{
    public class BoatHuntingLocomotionRatioController : ControllerInternal, ILateTickable
    {
        [Inject, UsedImplicitly] public BoatHuntingView view { get; }
        [Inject, UsedImplicitly] public MainCharacterSimpleWalkerController simpleWalkerController { get; }

        private float _prev = -1;
        
        private LocomotionCharacterMovingModule _locomotionMovingModule;

        public override void Construct()
        {
            base.Construct();
            _locomotionMovingModule = view.GetModule<LocomotionCharacterMovingModule>();
        }

        public void LateTick()
        {
            var ratio = (_locomotionMovingModule.isMoving 
                    ? (float)Math.Round(view.rigidbody.velocity.magnitude / simpleWalkerController.movementSpeed, 2) : 0);
            if (Math.Abs(ratio - _prev) > 0.01f)
            {
                _prev = ratio;
                _locomotionMovingModule.SetRatio(ratio);
            }
        }
    }
}