using GameCore.GameScene.LevelItems.Character.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene.LevelItems.InteractItems
{
    public class SeaInteractItem : InteractItem
    {
        public override int priority { get; } = 1;
        public override void OnEnter(InteractorCharacterModel interactorModel)
        {
            base.OnEnter(interactorModel);
            SwimModule swimModule = interactorModel.view.GetModule<SwimModule>();
            if (swimModule != null)
            {
                swimModule.SetSwim();
            }
        }
        
        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            base.OnExit(interactorModel);
            
            SwimModule swimModule = interactorModel.view.GetModule<SwimModule>();
            if (swimModule != null)
            {
                swimModule.SetNotSwim();
            }
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            
        }
    }
}
