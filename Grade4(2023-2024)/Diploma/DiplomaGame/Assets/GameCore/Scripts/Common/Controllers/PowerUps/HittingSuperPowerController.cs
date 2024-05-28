using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.Common.Misc;
using GameCore.Common.Settings;

namespace GameCore.Common.Controllers.PowerUps
{
    public class HittingSuperPowerController : SuperPowerController
    {
        public override string cheatBtnName => "HittingUp";
        protected override PowerUpsSettings.SuperPowerData superPowerData =>
            PowerUpsSettings.def.hittingSuperPowerData;
        
        protected override string rvPlacementName => CommStr.Rv_HittingSuperPower;
        protected override BuyObject activateBuyObject => powerUpsLevelManager.hittingSuperPowerAvailable;
        protected override string savePropertyOnceShownName => "OnceShownHittingSuperPower";
        protected override string savePropertyOnceClaimedName => "OnceClaimedHittingSuperPower";
        private int _hitMultiplier = 3;
        protected override void PowerEffect()
        {
            interactorCharactersCollection.mainCharacterView.GetModule<HittingCharacterModule>().SetHitMultiplier(_hitMultiplier);
            interactorCharactersCollection.mainCharacterView.GetModule<HittingCharacterModule>().OnSuperPower();
        }
        
        protected override void EndPowerEffect()
        {
            interactorCharactersCollection.mainCharacterView.GetModule<HittingCharacterModule>().ResetHitMultiplier(); 
            interactorCharactersCollection.mainCharacterView.GetModule<HittingCharacterModule>().OnEndSuperPower();
            powerUp = null;
        }
    }
}