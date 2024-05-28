using GameCore.GameScene_Island.LevelItems.Animal.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;

namespace GameCore.Common.LevelItems.Animals
{
    public class AnimalView : CharacterView
    {
        private AnimalAnimationsModule _animationsModuleCached;
        public AnimalAnimationsModule animationsModule => _animationsModuleCached ??=
            GetComponentInChildren<AnimalAnimationsModule>(true);
        
        private AnimalTaskModule _taskModuleCached;
        public AnimalTaskModule taskModule => _taskModuleCached ??= GetComponentInChildren<AnimalTaskModule>(true);

        private LocomotionAnimalMovingModule _locomotionMovingModuleCached;
        public LocomotionAnimalMovingModule locomotionMovingModule => _locomotionMovingModuleCached ??= GetComponentInChildren<LocomotionAnimalMovingModule>(true);

        private AnimalSpeedModule _speedModuleCached;
        public AnimalSpeedModule speedModule => _speedModuleCached ??= GetComponentInChildren<AnimalSpeedModule>(true); 
    }
}