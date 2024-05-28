using System.Collections.Generic;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.Common.Misc
{
    public class ContainerAnimalProducts : MonoBehaviour
    {
        private List<SellProduct> _products = new List<SellProduct>();
        public List<SellProduct> products => _products;
        
        public readonly TheSignal onAddedProduct = new();
        
        public void AddProduct(SellProduct prod)
        {
            _products.Add(prod);
            onAddedProduct.Dispatch();
            prod.onAddedToCarrier.Once(() =>
            {
                _products.Remove(prod);
            });
        }
    }
}