using System.Collections.Generic;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal
{
    public class AnimalProductingInteractItem : InteractItem
    {
        public override int priority { get; } = 2;
        
        private AnimalProductingView _viewCached;
        public AnimalProductingView view => _viewCached ??= GetComponentInParent<AnimalProductingView>();

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (CanInteract(interactorModel)) 
            {
                view.productionModule.GetProducts(interactorModel);
            }
        }

        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return base.CanInteract(interactorModel) && interactorModel.view.GetModule<AnimalInteractModule>() != null && interactorModel.view.GetModule<AnimalInteractModule>().CanInteract();;
        }
    }
}