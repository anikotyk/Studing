using GameCore.Common.LevelItems.Character.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.Common.LevelItems.Animals.FightAnimal.Modules
{
    public class FightAnimalFollowInteractItem : InteractItem
    {
        private FightAnimalFollowModule _followModuleCached;
        public FightAnimalFollowModule followModule
        {
            get
            {
                if (_followModuleCached == null) _followModuleCached = GetComponent<FightAnimalFollowModule>();
                return _followModuleCached;
            }
        }
        
        public override int priority { get; } = 3;

        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return base.CanInteract(interactorModel) && interactorModel.view.GetModule<GetDamageCharacterModule>();
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            followModule.OnEnterCharacter(interactorModel);
        }
        
        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            followModule.OnExitCharacter(interactorModel);
        }
    }
}