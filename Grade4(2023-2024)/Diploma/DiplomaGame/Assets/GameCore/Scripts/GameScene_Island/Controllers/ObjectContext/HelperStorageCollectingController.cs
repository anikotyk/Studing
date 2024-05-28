using GameCore.GameScene.Audio;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.Controllers.RoomContext.Common;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Units.Modules;
using Zenject;

namespace GameCore.GameScene_Island.Controllers.ObjectContext
{
    public class HelperStorageCollectingController : CollectingModuleController
    {
        [Inject, PublicAPI]
        public void SetCollectingModule(CollectingModule collectingModule)
        {
            SetModule(collectingModule);

            OnInitialize();
            onCollectProductStart.On(OnCollectStart);
        }

        private void OnCollectStart(ProductView product, bool animate)
        {
            if (product is SellProduct sellProduct)
            {
                sellProduct.interactItem.enabled = false;
                sellProduct.TurnOffOutline();
            }
        }
    }
}