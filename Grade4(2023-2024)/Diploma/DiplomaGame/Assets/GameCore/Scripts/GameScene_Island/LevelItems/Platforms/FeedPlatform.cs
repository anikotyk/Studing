using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Platforms
{
    public class FeedPlatform : SellProductsCollectPlatform
    {
        [SerializeField] private Transform _eatPoint;
        public Transform eatPoint => _eatPoint;
        
       public TheSignal onFullCarrier { get; } = new();
        public TheSignal onHasSpace { get; } = new();

        private void Awake()
        { 
            productsCarrier.onChange.On(() =>
            {
                if (IsHasSpace())
                {
                    onHasSpace.Dispatch();
                }
                else if (!productsCarrier.HasSpace())
                {
                    TurnOffInteractItem();
                    onFullCarrier.Dispatch();
                }
            });
        }

        public void TurnOffInteractItem()
        {
            interactItem.enabled = false;
        }
        
        public void TurnOnInteractItem()
        {
            interactItem.enabled = true;

            if (IsHasSpace())
            {
                onHasSpace.Dispatch();
            }
        }

        public bool IsHasSpace()
        {
            return productsCarrier.HasSpace() && interactItem.enabled;
        }
    }
}