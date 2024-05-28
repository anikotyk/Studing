using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class AnimalAvoidCharacterInteractItem : InteractItem
    {
        private AnimalAvoidCharacterModule _animalAvoidCharacterModule;
        public AnimalAvoidCharacterModule animalAvoidCharacterModule
        {
            get
            {
                if (_animalAvoidCharacterModule == null) _animalAvoidCharacterModule = GetComponent<AnimalAvoidCharacterModule>();
                return _animalAvoidCharacterModule;
            }
        }
        
        public override int priority { get; } = 3;

      
        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is MainCharacterModel)
            {
                animalAvoidCharacterModule.OnEnterCharacter();
            }
        }
    }
}