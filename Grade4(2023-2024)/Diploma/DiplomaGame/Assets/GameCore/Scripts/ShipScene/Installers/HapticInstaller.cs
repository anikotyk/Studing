using GameBasicsCore.Game.API;
using GameBasicsCoreModules.GameBasicsCore.Module.Haptic;
using Zenject;

namespace GameCore.ShipScene.Installers
{
    public class HapticInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IHapticService>().To<HapticService>().AsSingle().NonLazy();
        }
    }
}