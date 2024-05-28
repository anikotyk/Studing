using GameCore.Common.LevelItems.Character.Modules;
using GameCore.GameScene.Controllers.ObjectContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.InteractItems
{
    public class BuyPlatformInteractItem : CollectingModuleInteractItem<BuyCollectingController>
    {
        public override int priority { get; } = 1;
        
        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return collectingController?.CanInteract(interactorModel) == true && interactorModel.view.GetModule<InteractionCharacterModule>().CanInteract();
        }
        
        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if(!interactorModel.view.GetModule<InteractionCharacterModule>().CanInteract()) return;
            
            /*collectingController.onInteract.Once(() =>
            {
                interactorModel.view.GetModule<InteractionCharacterModule>().OnInteractionStart(this);
            });
            
            collectingController.onDoneTakingOut.Once(() =>
            { 
                interactorModel.view.GetModule<InteractionCharacterModule>().OnInteractionEnd(this);
            });*/
            
            base.Interact(interactorModel);
        }
        
        public override void StopInteraction(InteractorCharacterModel interactorModel)
        {
            base.StopInteraction(interactorModel);
            
            //interactorModel.view.GetModule<InteractionCharacterModule>().OnInteractionEnd(this);
        }
    }
}


