using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.Controllers.RoomContext.Common;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSDK.Modules.IdleArcade.Misc;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene_Island.Controllers.ObjectContext
{
    public abstract class CollectingModuleControllerSelling : CollectingModuleController
    {
        private Dictionary<ProductsCarrier, Coroutine> _takingOutCarriersCoroutines = new Dictionary<ProductsCarrier, Coroutine>();
        
        public void StopInteraction(ProductsCarrier carrier)
        {
            if (!_takingOutCarriersCoroutines.ContainsKey(carrier)) return;
            uep.StopCoroutine(_takingOutCarriersCoroutines[carrier]);
            _takingOutCarriersCoroutines.Remove(carrier);
            onStopInteract.Dispatch();
        }

        public void Interact(ProductsCarrier carrier)
        {
            if (_takingOutCarriersCoroutines.ContainsKey(carrier))
            {
                Debug.LogWarning($"Already running taking out for carrier {carrier.name}");
                return;
            }

            var coroutine = uep.StartCoroutine(RunTakingOut(carrier));
            if (coroutine == null) return;
            _takingOutCarriersCoroutines.Add(carrier, coroutine);
            onInteract.Dispatch();
        }
        
        private IEnumerator RunTakingOut(ProductsCarrier carrier)
        {
            var takingOutDelay = module.takingOutDelay;
            var wfs = new WaitForSeconds(takingOutDelay);
            while (!carrier.IsEmpty())
            {
                foreach (var productConfig in module.acceptableProducts)
                {
                    if (module.productsCarriers.IsAccepting(productConfig))
                    {
                        var product = carrier.GetLast(productConfig);
                        if (product == null) continue;
                        bool res = TryCollect(carrier, product);
                        if (res) break;
                    }
                }

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
        
        public bool TryCollect(ProductsCarrier productsCarrier, ProductView product, bool animate = true)
        {
            var carrier = module.productsCarriers.First(c => c.IsAccepting(product.id));
            if (!carrier.HasSpace())
            {
                return false;
            }

            productsCarrier.GetOut(product);
            onCollectProductStart.Dispatch(product, animate);
            carrier.Add(product, animate, () => onCollectProductComplete.Dispatch(product, animate));

            return true;
        }
    }
}