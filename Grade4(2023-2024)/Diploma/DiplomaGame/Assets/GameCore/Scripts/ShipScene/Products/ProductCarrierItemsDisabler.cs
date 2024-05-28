using System;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.ShipScene.Products
{
    public class ProductCarrierItemsDisabler : MonoBehaviour
    {
        [SerializeField] private ProductsCarrier _productsCarrier;

        private void OnEnable()
        {
            _productsCarrier.onAddComplete.On(OnAddComplete);
        }

        private void OnAddComplete(ProductView view, bool animate)
        {
            _productsCarrier.GetOut(view);
            view.Release();
        }
    }
}