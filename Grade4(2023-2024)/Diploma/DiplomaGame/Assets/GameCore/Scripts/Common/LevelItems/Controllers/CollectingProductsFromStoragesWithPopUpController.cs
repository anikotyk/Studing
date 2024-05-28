using GameCore.GameScene.Audio;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.Controllers.CharacterContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using Zenject;

namespace GameCore.Common.LevelItems.Controllers
{
    public class CollectingProductsFromStoragesWithPopUpController : CollectingProductsFromStoragesController
    {
        [Inject, UsedImplicitly] public ResourcesPopUpsController resourcesPopUpsController { get; }
        [Inject, UsedImplicitly] public PopSoundManager popSoundManager { get; }
        
        protected override void OnProductAddToCarrier(ProductView product)
        {
            resourcesPopUpsController.SpawnPopUpGetResource(product);
            popSoundManager.PlaySound();
        }
    }
}