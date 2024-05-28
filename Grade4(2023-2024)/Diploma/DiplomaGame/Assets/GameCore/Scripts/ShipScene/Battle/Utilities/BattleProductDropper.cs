using System.Collections.Generic;
using GameCore.ShipScene.Battle.Waves;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.ShipScene.Battle.Utilities
{
    public class BattleProductDropper : BattleStartEndListener
    {
        [SerializeField] private ProductsCarrier _productsCarrier;
        protected override void OnBattleStarted(Wave wave)
        {
            var productCache = new List<ProductView>(_productsCarrier.products);
            foreach (var product in productCache)
            {
                _productsCarrier.GetOut(product);
                product.Release();
            }
        }

        protected override void OnBattleEnded(Wave wave)
        {
        }
    }
}