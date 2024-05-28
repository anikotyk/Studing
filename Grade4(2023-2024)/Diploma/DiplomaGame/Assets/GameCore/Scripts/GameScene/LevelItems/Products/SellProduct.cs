using EPOOutline;
using GameCore.Common.Misc;
using GameCore.GameScene.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Products
{
    public class SellProduct : ProductView
    {
        [SerializeField] private ProductPriceDataConfig _priceDataConfig;
        public ProductPriceDataConfig priceDataConfig => _priceDataConfig;
        public SignalHub hub { get; private set; }

        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }
        
        private Collider _colliderCached;
        public Collider productCollider
        {
            get
            {
                if (_colliderCached == null) _colliderCached = GetComponentInChildren<Collider>();
                return _colliderCached;
            }
        }

        private Outlinable _outlinableCached;
        public Outlinable outlinable
        {
            get
            {
                if (_outlinableCached == null) _outlinableCached = GetComponent<Outlinable>();
                return _outlinableCached;
            }
        }

        public readonly TheSignal onSpend = new();
        public readonly TheSignal onAddedToCarrier = new();
        public readonly TheSignal onReturn = new();

        public bool isInCarrier { get; private set; }
        
        public virtual void Initialize(SignalHub signalHub)
        {
            hub = signalHub;
        }

        public override void OnAddToCarrierStart(ProductsCarrier carrier)
        {
            isInCarrier = true;
            onAddedToCarrier.Dispatch();
            interactItem.enabled = false;
        }

        public override void OnAddToCarrierComplete(ProductsCarrier carrier)
        {
            if (carrier.container.GetComponentInChildren<InvisibleContainer>(true))
            {
                transform.SetParent(carrier.container.GetComponentInChildren<InvisibleContainer>(true).transform);
            }
        }

        public override void OnReturnToStorage(ProductStorage storage)
        {
            isInCarrier = false;
            onReturn.Dispatch();
        }
        
        public void TurnOnOutline()
        {
            if (outlinable != null)
            {
                outlinable.enabled = true;
            }
        }
        
        public void TurnOffOutline()
        {
            if (outlinable != null)
            {
                outlinable.enabled = false;
            }
        }

        public virtual void ResetView()
        {
            TurnOffOutline();
            interactItem.enabled = false;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            isInCarrier = false;
        }

        public virtual void TurnOnInteractItem()
        {
            interactItem.enabled = true;
            productCollider.enabled = true;
        }
        
        public virtual void TurnOffInteractItem()
        {
            interactItem.enabled = false;
            productCollider.enabled = false;
        }
    }
}