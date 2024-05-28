using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Platforms;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.InteractItems
{
    public class BuySCPlatformInteractItem : InteractItem
    {
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [SerializeField] private DonateResourcesPlatform _donateResourcesPlatform;
        public override int priority { get; } = 1;

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (buyObjectsManager.isCheatsBuyEnabled && interactorModel.view is MainCharacterView && _donateResourcesPlatform.interactItem.enabled)
            {
                _donateResourcesPlatform.DonateComplete();
                return;
            }
            
            if (interactorModel.view is MainCharacterView)
            {
                _donateResourcesPlatform.TryUseSC();
            }
        }
    }
}