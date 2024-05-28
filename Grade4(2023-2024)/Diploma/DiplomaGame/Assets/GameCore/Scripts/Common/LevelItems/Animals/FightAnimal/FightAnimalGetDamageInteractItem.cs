using GameCore.Common.LevelItems.Animals.FightAnimal.Modules;
using GameCore.Common.LevelItems.Character.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.Common.LevelItems.Animals.FightAnimal
{
    public class FightAnimalGetDamageInteractItem : InteractItem
    {
        public override int priority { get; } = 5;
        
        private FightAnimalGetDamageModule _fightAnimalGetDamageModuleCached;
        public FightAnimalGetDamageModule fightAnimalGetDamageModule
        {
            get
            {
                if (_fightAnimalGetDamageModuleCached == null) _fightAnimalGetDamageModuleCached = GetComponent<FightAnimalGetDamageModule>();
                return _fightAnimalGetDamageModuleCached;
            }
        }
        
        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return base.CanInteract(interactorModel) && interactorModel.view.GetModule<HittingCharacterModule>() && interactorModel.view.GetModule<HittingCharacterModule>().CanInteract(fightAnimalGetDamageModule.toolType);
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (!fightAnimalGetDamageModule.isEnabled) return;
            interactorModel.view.GetModule<HittingCharacterModule>().EnteredHittableItem(fightAnimalGetDamageModule);
        }

        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            interactorModel.view.GetModule<HittingCharacterModule>().ExitedHittableItem(fightAnimalGetDamageModule);
        }
    }
}