using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Iceland.LevelItems.Items.InteractItems
{
    public class DangerZoneInteractItem : InteractItem
    {
        public override int priority { get; } = 5;
        
        private DangerZone _dangerZoneCached;
        public DangerZone dangerZone => _dangerZoneCached ??= GetComponentInParent<DangerZone>(true);
        
        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (interactorModel.view is not BoatHuntingView) return;
            dangerZone.OnEnterZone(interactorModel.view as BoatHuntingView);
        }

        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            base.OnExit(interactorModel);
            if (interactorModel.view is not BoatHuntingView) return;
            dangerZone.OnExitZone(interactorModel.view as BoatHuntingView);
        }
    }
}