using GameCore.Common.LevelItems;
using GameCore.Common.Misc;
using GameCore.Common.Settings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Capacity;

namespace GameCore.Common.Controllers.PowerUps
{
    public class CapacitySuperPowerController : SuperPowerController
    {
        public override string cheatBtnName => "CapacityUp";
        protected override PowerUpsSettings.SuperPowerData superPowerData =>
            PowerUpsSettings.def.capacitySuperPowerData;

        protected override string rvPlacementName => CommStr.Rv_CapacitySuperPower;
        
        protected override string savePropertyOnceShownName => "OnceShownCapacitySuperPower";
        protected override string savePropertyOnceClaimedName => "OnceClaimedCapacitySuperPower";
        protected override BuyObject activateBuyObject => powerUpsLevelManager.capacitySuperPowerAvailable;
        
        protected override void PowerEffect()
        {
            interactorCharactersCollection.mainCharacterView.GetModule<InteractorCapacityModule>().MultiplyCustomCapacity(3);
        }
        
        protected override void EndPowerEffect()
        {
            interactorCharactersCollection.mainCharacterView.GetModule<InteractorCapacityModule>().ResetCustomCapacity();
            powerUp = null;
        }
    }
}
