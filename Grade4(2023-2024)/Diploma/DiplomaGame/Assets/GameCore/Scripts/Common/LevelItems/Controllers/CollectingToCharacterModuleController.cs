using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.Controllers.RoomContext.Common;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Units.Modules;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Controllers
{
    public class CollectingToCharacterModuleController : CollectingModuleController
    {
        private Dictionary<InteractorCharacterModel, Coroutine> _takingOutCoroutines;

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
        
        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            if (!allowToCollect) return false;
            
            foreach (var carrier in module.productsCarriers)
            {
                if(!carrier.IsEmpty()) return true;
            }
            
            return false;
        }

        protected override IEnumerator RunTakingOut(InteractorCharacterModel interactorModel)
        {
            var takingOutDelay = module.takingOutDelay;
            var wfs = new WaitForSeconds(takingOutDelay);
            while (CanInteract(interactorModel))
            {
                bool gotProd = false;
                foreach (var carrier in  module.productsCarriers)
                {
                    for (int i = carrier.products.Count - 1; i >= 0; i--)
                    {
                        //var prod = carrier.products.LastOrDefault((prod) => interactorModel.productCarriersController.productsCarriers.IsAccepting(prod.id));
                        var prod = carrier.products[i];
                        if (prod)
                        {
                            gotProd = TryCollect(interactorModel, carrier, prod);
                            if (gotProd) break;
                        }
                    }
                    if (gotProd) break;
                }
               
                
                /*foreach (var productConfig in module.acceptableProducts)
                {
                    if (interactorModel.productCarriersController.productsCarriers.IsAccepting(productConfig))
                    {
                        //var carrier = module.productsCarriers.GetBy(productConfig);
                        var product = carrier.GetFirst(productConfig);
                        if (product == null) continue;
                        bool res = TryCollect(interactorModel, carrier, product);
                        if (res) break;
                    }
                }*/

                yield return wfs;
                
                if (module.IsProgressiveTakingOutDelay())
                {
                    takingOutDelay *= module.progressiveTakingOutDelayRatio;
                    takingOutDelay = takingOutDelay.MinClamp(module.minTakingOutDelay);
                    wfs = new WaitForSeconds(takingOutDelay);
                }
            }
            
            onDoneTakingOut.Dispatch();
            
        }

        public bool TryCollect(InteractorCharacterModel interactorModel, ProductsCarrier productCarrier, ProductView product, bool animate = true)
        {
            var carrier = interactorModel.productCarriersController.productsCarriers.First(c => c.IsAccepting(product.id));
            if (!carrier.HasSpace())
            {
                return false;
            }

            productCarrier.GetOut(product);
            onCollectProductStartInteractor.Dispatch(interactorModel);
            onCollectProductStart.Dispatch(product, animate);
            carrier.Add(product, animate, () => onCollectProductComplete.Dispatch(product, animate));

            return true;
        }
    }
}