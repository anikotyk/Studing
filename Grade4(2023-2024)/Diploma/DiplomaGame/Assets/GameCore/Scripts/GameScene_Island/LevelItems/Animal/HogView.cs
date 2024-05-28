using GameCore.Common.LevelItems.Animals;
using GameCore.GameScene_Island.LevelItems.Animal.Modules;

namespace GameCore.GameScene_Island.LevelItems.Animal
{
    public class HogView : AnimalView
    {
        private AnimalInTrapModule _animalInTrapModuleCached;
        public AnimalInTrapModule animalInTrapModule => _animalInTrapModuleCached ??= GetComponentInChildren<AnimalInTrapModule>(true);
        
        private HogLogicModule _hogLogicModuleCached;
        public HogLogicModule hogLogicModule => _hogLogicModuleCached ??= GetComponentInChildren<HogLogicModule>(true);
    }
}