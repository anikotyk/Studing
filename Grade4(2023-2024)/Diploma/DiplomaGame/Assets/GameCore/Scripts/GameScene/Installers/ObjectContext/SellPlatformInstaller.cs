using GameCore.GameScene.Controllers.ObjectContext;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Units.Modules;
using Zenject;

namespace GameCore.GameScene.Installers.ObjectContext
{
    public class SellPlatformInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindDefault<SellCollectingController>();
            Container.BindInstance(GetComponent<CollectingModule>());
        }
    }
}