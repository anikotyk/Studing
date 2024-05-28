using GameCore.GameScene_Island.Controllers.ObjectContext;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Units.Modules;
using Zenject;

namespace GameCore.GameScene_Island.Installers.ObjectContext
{
    public class HelperStoragePlatformInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindDefault<HelperStorageCollectingController>();
            Container.BindInstance(GetComponent<CollectingModule>());
        }
    }
}