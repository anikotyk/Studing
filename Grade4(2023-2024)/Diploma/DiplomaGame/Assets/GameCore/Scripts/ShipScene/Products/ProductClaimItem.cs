using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using UnityEngine;

namespace GameCore.ShipScene.Products
{
    
    public class ProductClaimItem : MonoBehaviour
    {
        [SerializeField] private ProductView _productView;

        public void Claim()
        {
            _productView.Release();
        }
    }
}