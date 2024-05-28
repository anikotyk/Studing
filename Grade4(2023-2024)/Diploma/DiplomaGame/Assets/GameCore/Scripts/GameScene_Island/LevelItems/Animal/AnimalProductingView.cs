using GameCore.Common.LevelItems.Animals;
using GameCore.GameScene_Island.LevelItems.Animal.Modules;

namespace GameCore.GameScene_Island.LevelItems.Animal
{
    public class AnimalProductingView : AnimalView
    {
        private AnimalProductionModule _productionModuleCached;
        public AnimalProductionModule productionModule => _productionModuleCached ??= GetComponentInChildren<AnimalProductionModule>(true);
    }
}