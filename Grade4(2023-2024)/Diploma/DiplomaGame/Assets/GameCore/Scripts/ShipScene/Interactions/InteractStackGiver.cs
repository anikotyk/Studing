using System.Collections.Generic;
using System.Linq;
using GameCore.Common.LevelItems.Managers;
using GameCore.ShipScene.Common;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.Misc;
using StaserSDK.Interactable;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Interactions
{
    public class InteractStackGiver : MaxView
    {
        [SerializeField] private ZoneBase _zone;
        [SerializeField] private ProductDataConfig _targetProduct;
        [SerializeField] private Transform _spawnPoint;
        
        [Inject, UsedImplicitly] public ProductLimitManager productLimitManager { get; }
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }

        private List<ProductView> _cachedProducts = new();

        private int _freePlace = int.MaxValue;
        private bool _showedEnough = false;
        
        private void OnEnable()
        {
            _showedEnough = false;
            _zone.onEnter.On(OnEnter);
            _zone.onInteract.On(OnInteract);
            _zone.onExit.On(OnExit);
        }

        private void OnDisable()
        {
            _zone.onEnter.On(OnEnter);
            _zone.onInteract.Off(OnInteract);
            _zone.onExit.Off(OnExit);
        }

        private void OnEnter(InteractableCharacter character)
        {
            _showedEnough = false;
            var carrier = character.productsCarrier;
            _freePlace = productLimitManager.GetLimit(_targetProduct) - carrier.products.Count(x=>x.dataConfig.id == _targetProduct.id);
            if (_freePlace <= 0)
            {
                Enough(character.transform.position);
                return;
            }
            carrier.onAddComplete.On(OnProductAdded);
            if(carrier.HasSpace())
                return;
            Max(character.transform.position);
        }

        private void OnExit(InteractableCharacter character)
        {
            var carrier = character.productsCarrier;
            carrier.onAddComplete.Off(OnProductAdded);
            _cachedProducts.Clear();
        }

        private void OnInteract(InteractableCharacter character)
        {
            var carrier = character.productsCarrier;
            if(carrier.HasSpace() == false)
                return;
            if (_freePlace <= 0)
            {
                if (_showedEnough == false)
                {
                    _showedEnough = true;
                    Enough(character.transform.position);
                }
                return;
            }
            if(carrier.products.Count + _cachedProducts.Count >= carrier.capacity)
                return;
            _freePlace--;
            var spawnedProduct = spawnProductsManager.Spawn(_targetProduct, _spawnPoint.position);
            _cachedProducts.Add(spawnedProduct);
        }

        private void OnProductAdded(ProductView view, bool animate)
        {
            _cachedProducts.Remove(view);
            if(_freePlace <= 0 && _cachedProducts.Count <= 0)
                Enough(_spawnPoint.position);
        }
    }
}