using System.Threading.Tasks;
using GameCore.Common.LevelItems.Controllers;
using GameCore.Common.Misc;
using GameCore.GameScene.Audio;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Misc;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsSDK.Modules.IdleArcade.Misc;
using GameBasicsSDK.Modules.IdleArcade.Models;
using Zenject;

namespace GameCore.GameScene.Controllers.SceneContext.Products
{
    public class SellCollectController : ControllerInternal
    {
        [Inject, UsedImplicitly] public PopSoundManager popSoundManager { get; }
        [Inject, UsedImplicitly] public ResourcesPopUpsController resourcesPopUpsController { get; }
        
        public override void Construct()
        {
            base.Construct();

            hub.Get<GCSgnl.SellSignals.Interact>().On(OnInteract);
        }

        private async void OnInteract(InteractorCharacterModel interactorModel, SellProduct product)
        {
            var carrier = interactorModel.productCarriersController.GetCarrier(product.id);
            
            if(carrier==null) return;

            if (!carrier.HasSpace())
            {
                if (interactorModel is MainCharacterModel)
                {
                    hub.Get<IASgnl.Misc.Max>().Dispatch(carrier.transform.position); 
                }
                return;
            }
            
            if (carrier.Contains(product)) return;
            
            carrier.PreAdd(product);
            while (carrier.isWaitingSomeAnimationToComplete) await Task.Yield();

            if (product.TryGetComponent<SeaAnimatedProduct>(out SeaAnimatedProduct seaAnimatedProduct))
            {
                if (seaAnimatedProduct.isSeaAnimStarted)
                {
                    seaAnimatedProduct.EndAnims();
                }
            }
            
            if (interactorModel is MainCharacterModel)
            {
                SpawnPopUpGetResource(product);
            }
            carrier.Add(product, true, interactorModel.productCarriersController.onChange.Dispatch);

            if (interactorModel is MainCharacterModel)
            {
                popSoundManager.PlaySound();
            }

            while (carrier.isWaitingSomeAnimationToComplete) await Task.Yield();
        }

        private void SpawnPopUpGetResource(SellProduct product)
        {
            resourcesPopUpsController.SpawnPopUpGetResource(product);
        }
    }
}