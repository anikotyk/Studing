using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Character.Modules;

namespace GameCore.GameScene.Helper.Modules
{
    public class GetDamageHelperModule : GetDamageCharacterModule
    {
        private HelperView _viewCached;
        public HelperView view => _viewCached ??= GetComponentInParent<HelperView>(true);
        private HealthModule _healthModuleCached;
        public HealthModule healthModule => _healthModuleCached ??= view.GetComponentInChildren<HealthModule>(true);

        public override void Construct()
        {
            base.Construct();
            if (healthModule)
            {
                healthModule.onDied.On(() =>
                {
                    view.GetModule<HittingCharacterModule>().StopHitting();
                    view.logicModule.ResetHelper();
                    healthModule.ResetHealth();
                    healthModule.DeactivateHealth(true);
                });
            }
        }

        public override void GetDamage()
        {
            base.GetDamage();
            if (healthModule)
            {
                if (!healthModule.isPopUpActive)
                {
                    healthModule.ActivateHealth();
                    healthModule.ResetHealth();
                }

                healthModule.GetDamage();
            }
        }
    }
}