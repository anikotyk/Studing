using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Platforms;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene.LevelItems
{
    public class StallObject : InjCoreMonoBehaviour
    {
        [SerializeField] private Transform _acceptPoint;
        public Transform acceptPoint => _acceptPoint;

        private ProductsCarrier _carrierCached;
        public ProductsCarrier carrier
        {
            get
            {
                if (_carrierCached == null) _carrierCached = GetComponentInChildren<ProductsCarrier>();
                return _carrierCached;
            }
        }

        private SellPlatform _sellPlatformCached;
        public SellPlatform sellPlatform
        {
            get
            {
                if (_sellPlatformCached == null) _sellPlatformCached = GetComponentInChildren<SellPlatform>(true);
                return _sellPlatformCached;
            }
        }
        
        private BuyObject _buyObjectCached;
        public BuyObject buyObject
        {
            get
            {
                if (_buyObjectCached == null) _buyObjectCached = GetComponentInParent<BuyObject>(true);
                return _buyObjectCached;
            }
        }
    }
}