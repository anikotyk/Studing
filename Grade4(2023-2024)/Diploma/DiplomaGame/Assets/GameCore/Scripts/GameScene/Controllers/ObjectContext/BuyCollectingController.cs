using GameCore.GameScene.Audio;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.Controllers.RoomContext.Common;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Units.Modules;
using GameBasicsSDK.Modules.IdleArcade.Models;
using GameBasicsSignals;
using Zenject;

namespace GameCore.GameScene.Controllers.ObjectContext
{
    public class BuyCollectingController : CollectingModuleController
    {
        [Inject, UsedImplicitly] public SpotSoundManager spotSoundManager { get; }

        public TheSignal<ProductView> onAdd { get; } = new();

        [Inject, PublicAPI]
        public void SetCollectingModule(CollectingModule collectingModule)
        {
            SetModule(collectingModule);

            OnInitialize();
            onCollectProductStart.On(OnCollectStart);
        }

        private void OnCollectStart(ProductView product, bool animate)
        {
            onAdd?.Dispatch(product);
            
            if (product is SellProduct sellProduct)
            {
                sellProduct.interactItem.enabled = false;
            }
            
            spotSoundManager.PlaySound();
        }
    }
}