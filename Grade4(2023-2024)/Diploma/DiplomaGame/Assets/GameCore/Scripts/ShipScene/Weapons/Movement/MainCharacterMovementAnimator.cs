using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Misc.Refs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Weapons.Bullets.Movement
{
    public class MainCharacterMovementAnimator: WeaponIsShootingListener
    {
        [SerializeField] private AnimatorParameterApplier _backwardApplier;
        [Inject, UsedImplicitly] public MainCharacterSimpleWalkerController walker { get; }
        [Inject, UsedImplicitly] public MainCharacterView mainCharacterView { get; }
        [Inject, UsedImplicitly] public ScreenJoystickCharacterInput characterInput { get; }
        [Inject, UsedImplicitly] public MainCameraRef mainCameraRef { get; }

        private bool _injected = false;
        
        [Inject]
        public override void Construct()
        {
            _injected = true;
            _backwardApplier.SetAnimator(mainCharacterView.animator);
        }
        
        
        private Vector3 CalculateMovementDirection()
        {
            if (mainCameraRef == null) return Vector3.zero;
            
            Vector3 direction = Vector3.zero;
            
            var up = walker.transform.up;
            
            direction += Vector3.ProjectOnPlane(mainCameraRef.camera.transform.right, up).normalized
                         * characterInput.GetHorizontalMovementInput();
            
            direction += Vector3.ProjectOnPlane(mainCameraRef.camera.transform.forward, up).normalized
                         * characterInput.GetVerticalMovementInput();

            return direction;
        }

        private void Update()
        {
            if(_injected == false)
                return;
            
            var movementDirection = CalculateMovementDirection();
            if (isShooting == false || movementDirection == Vector3.zero)
            {
                MoveForward();
                return;
            }

            float dot = Vector3.Dot(movementDirection, walker.transform.forward);
            if (dot < 0)
                MoveBackward();
            else
                MoveForward();
        }

        private void MoveBackward()
        {
            _backwardApplier.ApplyAsBool(true);
        }

        private void MoveForward()
        {
            _backwardApplier.ApplyAsBool(false);
        }
        
    }
}