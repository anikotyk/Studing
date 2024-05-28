using System;
using GameCore.GameScene_Iceland.LevelItems.Character.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Items
{
    public class WarmZoneInteractItem : InteractItem
    {
        private void OnEnable()
        {
            GetComponentInChildren<Collider>(true).enabled = false;
            GetComponentInChildren<Collider>(true).enabled = true;
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            var coldModule = interactorModel.view.GetModule<MainCharacterColdModule>();
            if (coldModule)
            {
                coldModule.EnterWarmZone();
            }
        }

        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            var coldModule = interactorModel.view.GetModule<MainCharacterColdModule>();
            if (coldModule)
            {
                coldModule.ExitWarmZone();
            }
        }
    }
}