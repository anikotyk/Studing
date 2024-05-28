using GameCore.GameScene_Iceland.LevelItems.Items;
using GameCore.GameScene_Iceland.LevelItems.Managers;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Animals.KillerWhale
{
    public class WhaleKillerAttackingInteractItem : InteractItem
    {
        [Inject, UsedImplicitly] public SwimHuntingManager swimHuntingManager { get; }
        public override int priority { get; } = 5;
        
        private WhaleKillerView _viewCached;
        public WhaleKillerView view => _viewCached ??= GetComponentInParent<WhaleKillerView>(true);
        
        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (interactorModel.view is not BoatHuntingView) return;
            if (!swimHuntingManager.isHuntingNow) return;
            view.attackingModule.StartAttack(interactorModel.view as BoatHuntingView);
        }
    }
}