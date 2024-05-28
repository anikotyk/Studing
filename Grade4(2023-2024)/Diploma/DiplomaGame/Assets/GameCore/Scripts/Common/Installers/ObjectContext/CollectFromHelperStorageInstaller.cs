using GameCore.Common.LevelItems.Controllers;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Units.Modules;
using Zenject;

namespace GameCore.Common.Installers.ObjectContext
{
    public class CollectFromHelperStorageInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindDefault<CollectingToCharacterModuleController>();
            Container.BindInstance(GetComponent<CollectingModule>());
        }
    }
}