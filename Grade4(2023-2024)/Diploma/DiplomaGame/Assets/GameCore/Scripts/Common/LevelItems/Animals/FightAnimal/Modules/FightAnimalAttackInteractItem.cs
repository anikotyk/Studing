using GameCore.Common.LevelItems.Character.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.Common.LevelItems.Animals.FightAnimal.Modules
{
    public class FightAnimalAttackInteractItem : InteractItem
    {
        private FightAnimalAttackModule _attackModuleCached;
        public FightAnimalAttackModule attackModule
        {
            get
            {
                if (_attackModuleCached == null) _attackModuleCached = GetComponent<FightAnimalAttackModule>();
                return _attackModuleCached;
            }
        }
        
        public override int priority { get; } = 3;

        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return base.CanInteract(interactorModel) && interactorModel.view.GetModule<GetDamageCharacterModule>();
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            attackModule.OnEnterCharacter(interactorModel);
        }
        
        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            attackModule.OnExitCharacter(interactorModel);
        }
    }
}