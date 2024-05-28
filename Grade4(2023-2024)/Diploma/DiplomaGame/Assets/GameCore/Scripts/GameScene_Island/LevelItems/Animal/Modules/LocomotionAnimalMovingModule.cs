using GameCore.Common.LevelItems.Animals;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class LocomotionAnimalMovingModule : LocomotionCharacterMovingModule
    {
        private AnimalView _viewCached;
        public AnimalView view => _viewCached ??= GetComponentInParent<AnimalView>(true);
        
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