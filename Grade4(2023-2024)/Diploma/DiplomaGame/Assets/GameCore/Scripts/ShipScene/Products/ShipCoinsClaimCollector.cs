using GameCore.GameScene.LevelItems.Products;
using GameCore.ShipScene.Common;
using GameCore.ShipScene.Currency;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Products
{
    public class ShipCoinsClaimCollector : InjCoreMonoBehaviour
    {
        [SerializeField] private ProductsCarrier _productsCarrier;

        [Inject, UsedImplicitly] public ShipCoinsCollectModel shipCoinsCollectModel { get; }

        public ShipSceneSettings settings => ShipSceneSettings.def;

        public override void Construct()
        {
            _productsCarrier.onAddComplete.On(OnAddComplete);
        }

        private void OnAddComplete(ProductView view, bool animate)
        {
            shipCoinsCollectModel.Earn(((SellProduct)view).priceDataConfig.softCurrencyCount, settings.currencyModule.currencyName, view.dataConfig.id);
            view.Release();
        }
    }
}