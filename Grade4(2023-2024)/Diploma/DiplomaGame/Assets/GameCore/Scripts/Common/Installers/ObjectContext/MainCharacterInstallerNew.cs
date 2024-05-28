using GameCore.Common.LevelItems.Controllers;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.Controllers.CharacterContext;
using GameBasicsSDK.Modules.IdleArcade.Installers.CharacterContext;

namespace GameCore.Common.Installers.ObjectContext
{
    public class MainCharacterInstallerNew : MainCharacterInstaller
    {
        protected override void BindCollectingFromStorages()
        {
            Container.BindInterfacesAndBaseAndSelfTo<CollectingProductsFromStoragesController, 
                CollectingProductsFromStoragesWithPopUpController>().AsSingle().NonLazy();
        }
    }
}