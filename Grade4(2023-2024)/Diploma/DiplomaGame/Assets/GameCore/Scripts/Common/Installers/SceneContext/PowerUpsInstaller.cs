using GameCore.Common.Controllers.PowerUps;
using GameCore.Common.LevelItems.PowerUps;
using GameCore.Common.UI.PowerUps;
using GameBasicsCore.Tools.Extensions;
using Zenject;

namespace GameCore.Common.Installers.SceneContext
{
    public class PowerUpsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindDefault<PowerUpsController>();
            Container.BindDefault<PowerUpsLevelManager>().FromComponentsInHierarchy();
            Container.BindDefault<CurrencyPowerUpWindowManager>();
            Container.BindDefault<SuperPowerTimersContainer>().FromComponentsInHierarchy();
            Container.BindDefault<PowerUpsOfferContainer>().FromComponentsInHierarchy();
            Container.BindDefault<PowerUpsCheatsController>();

            BindSuperPower();
            BindStackOfCash();
        }

        private void BindSuperPower()
        {
            Container.BindDefault<CapacitySuperPowerController>();
            Container.BindDefault<SpeedSuperPowerController>();
            Container.BindDefault<HittingSuperPowerController>();
            Container.BindDefault<ProductionSuperPowerController>();
            Container.BindDefault<AutoWateringController>();
            
            Container.BindDefault<SuperPowerWindowManager>();
        }
        
        private void BindStackOfCash()
        {
            Container.BindDefault<StackOfCashController>();
        }
    }
}
