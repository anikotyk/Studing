using GameCore.GameScene_Iceland.LevelItems.Items;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.Controllers
{
    public class JoystickBoatInputController : ControllerInternal, IJoystickWorldAgent
    {
        [Inject, UsedImplicitly] public ScreenJoystickCharacterInput joystickCharacterInput { get; }
        [Inject, UsedImplicitly] public BoatHuntingView boatHuntingView { get; }
      
        private LocomotionCharacterMovingModule _movingModule;
        public override void Construct()
        {
            _movingModule = boatHuntingView.GetModule<LocomotionCharacterMovingModule>();
        }

       public void JoystickStartUsing()
        {
            _movingModule.StartMoving();
        }

        public void JoystickStopUsing()
        {
            joystickCharacterInput.SetJoystickDirection(Vector3.zero);
            _movingModule.StopMoving();
        }

        public void UpdateJoystickDirection(Vector3 direction)
        {
            joystickCharacterInput.SetJoystickDirection(direction);
        }
    }
}