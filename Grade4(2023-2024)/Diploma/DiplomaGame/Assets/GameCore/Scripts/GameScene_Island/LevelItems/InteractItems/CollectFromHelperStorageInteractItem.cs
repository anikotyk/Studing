using System.Linq;
using GameCore.Common.LevelItems.Controllers;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class CollectFromHelperStorageInteractItem : CollectingModuleInteractItem<CollectingToCharacterModuleController>
    {
        public override int priority { get; } = 3;
        
        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return base.CanInteract(interactorModel) && interactorModel is MainCharacterModel && interactorModel.productCarriersController.productsCarriers.FirstOrDefault(carrier=> carrier.HasSpace());
        }
    }
}