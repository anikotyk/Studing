using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using UnityEngine;

namespace GameCore.GameScene.Helper.Modules
{
    public class LocomotionHelperMovingModule : LocomotionCharacterMovingModule
    {
        private HelperView _viewCached;
        public HelperView view => _viewCached ??= GetComponentInParent<HelperView>();
        
        public void StartMovement()
        {
            if (!view.taskModule.aiPath.canMove)
            {
                return;
            }
            SetRatio(Mathf.Clamp01(view.speedModule.speed / view.speedModule.maxSpeed));
            StartMoving();
        }

        public void StopMovement()
        {
            SetRatio(0);
            StopMoving();
        }
    }
}