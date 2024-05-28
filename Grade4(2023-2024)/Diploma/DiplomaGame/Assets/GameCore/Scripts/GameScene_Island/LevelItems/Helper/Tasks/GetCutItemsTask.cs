using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Helper.Tasks
{
    public class GetCutItemsTask : HelperTask
    {
        private HelperView _view;
        private InteractorCharacterProductsCarrier _carrier;

        private CuttableItem[] _cuttableItemsCached;

        public CuttableItem[] cuttableItems
        {
            get
            {
                if (_cuttableItemsCached == null) _cuttableItemsCached = GameObject.FindObjectsOfType<CuttableItem>(true).ToList().FindAll(item => cuttingModule.IsAbleToCut(item.spawnProductConfig)).ToArray();
                return _cuttableItemsCached;
            }
        }
        
        private HelperTasksGroup _tasksGroup;

        private List<SellProduct> _usedProducts = new List<SellProduct>();

        public CuttingModule cuttingModule => _view.GetModule<CuttingModule>();
        
        public void Initialize(HelperView view, InteractorCharacterProductsCarrier carrier)
        {
            _view = view;
            _carrier = carrier;
            
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }

        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                if (!_carrier.HasSpace())
                {
                    break;
                }
                
                var product = GetReadyProduct();
                if(product == null) break;
                _usedProducts.Add(product);
                yield return null;
                Vector3 pos = product.transform.position;
                pos.y = 0;
                _view.taskModule.MoveTo(product.name, pos);
            
                yield return null;
                float timer = 0;
                while (!_view.taskModule.aiPath.reachedDestination && _carrier.HasSpace() &&
                       product.interactItem.enabled && timer < maxTimeMoveToPoint)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
                _view.locomotionMovingModule.StopMovement();
            }
        }

        private SellProduct GetReadyProduct()
        {
            foreach (var item in cuttableItems)
            {
                foreach (var prod in item.productsAvailableForPickUp)
                {
                    if (prod.interactItem.enabled && !_usedProducts.Contains(prod))
                    {
                        return prod;
                    }
                }
            }
            
            return null;
        }
        
        protected override void StopInternal()
        {
            base.StopInternal();
            _tasksGroup.StopTask();
            _usedProducts.Clear();
        }
    }
}