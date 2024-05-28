using System.Linq;
using GameCore.GameScene_Island.Controllers.ObjectContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class HelperStorageInteractItem : CollectingModuleInteractItem<HelperStorageCollectingController>
    {
        public override int priority { get; } = 3;
        
        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return base.CanInteract(interactorModel) && interactorModel is not MainCharacterModel;
        }

    }
}