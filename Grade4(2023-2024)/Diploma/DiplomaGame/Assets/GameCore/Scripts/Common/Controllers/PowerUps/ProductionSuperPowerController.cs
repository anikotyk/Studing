using GameCore.Common.LevelItems;
using GameCore.Common.Misc;
using GameCore.Common.Settings;
using JetBrains.Annotations;
using Zenject;

namespace GameCore.Common.Controllers.PowerUps
{
    public class ProductionSuperPowerController : SuperPowerController
    {
        [Inject, UsedImplicitly] public ProductionController productionController { get; }
        
        public override string cheatBtnName => "ProductionUp";

        protected override string rvPlacementName => CommStr.Rv_ProductionSuperPower;
        protected override PowerUpsSettings.SuperPowerData superPowerData =>
            PowerUpsSettings.def.productionSuperPowerData;
        protected override BuyObject activateBuyObject => powerUpsLevelManager.productionSuperPowerAvailable;
        protected override string savePropertyOnceShownName => "OnceShownProductionSuperPower";
        protected override string savePropertyOnceClaimedName => "OnceClaimedProductionSuperPower";
        private int _productionMultiplier = 2;
        protected override void PowerEffect()
        {
            productionController.SetProductionMultiplier(_productionMultiplier);
        }
        
        protected override void EndPowerEffect()
        {
            productionController.ResetProductionMultiplier();
            powerUp = null;
        }
    }
}