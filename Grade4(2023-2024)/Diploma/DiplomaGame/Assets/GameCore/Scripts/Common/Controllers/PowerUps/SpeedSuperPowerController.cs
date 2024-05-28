using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.Common.Misc;
using GameCore.Common.Settings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Speeds;

namespace GameCore.Common.Controllers.PowerUps
{
    public class SpeedSuperPowerController : SuperPowerController
    {
        public override string cheatBtnName => "SpeedUp";
        protected override PowerUpsSettings.SuperPowerData superPowerData =>
            PowerUpsSettings.def.speedSuperPowerData;
        protected override string rvPlacementName => CommStr.Rv_SpeedSuperPower;
        
        protected override BuyObject activateBuyObject => powerUpsLevelManager.speedSuperPowerAvailable;
        
        protected override string savePropertyOnceShownName => "OnceShownSpeedSuperPower";
        protected override string savePropertyOnceClaimedName => "OnceClaimedSpeedSuperPower";

        private float _speedMultiplier = 1.25f;
        
        protected override void PowerEffect()
        {
            interactorCharactersCollection.mainCharacterView.GetModule<MainCharacterSpeedModule>().SetSpeedMultiplier(_speedMultiplier);
            interactorCharactersCollection.mainCharacterView.GetModule<SpeedSuperPowerModule>().OnSuperPower();
        }
        
        protected override void EndPowerEffect()
        {
            interactorCharactersCollection.mainCharacterView.GetModule<MainCharacterSpeedModule>().ResetSpeedMultiplier();
            interactorCharactersCollection.mainCharacterView.GetModule<SpeedSuperPowerModule>().OnEndSuperPower();
            
            powerUp = null;
        }
    }
}