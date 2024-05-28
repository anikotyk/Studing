using GameCore.GameScene_Island.Controllers.ObjectContext;
using GameCore.GameScene.Audio;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.Controllers.RoomContext.Common;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Units.Modules;
using GameBasicsSDK.Modules.IdleArcade.Models;
using Zenject;

namespace GameCore.GameScene.Controllers.ObjectContext
{
    public class SellCollectingController : CollectingModuleControllerSelling
    {
        [Inject, UsedImplicitly] public StallSoundManager stallSoundManager { get; }
        
        [Inject, PublicAPI]
        public void SetCollectingModule(CollectingModule collectingModule)
        {
            SetModule(collectingModule);

            OnInitialize();
            onCollectProductStart.On(OnCollectStart);
            onCollectProductStartInteractor.On((interactor) =>
            {
                if (interactor is MainCharacterModel)
                {
                    stallSoundManager.PlaySound();
                }
            });
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