using System.Collections;
using System.Collections.Generic;
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
    public class SellCutItemsTask : HelperTask
    {
        private HelperView _view;
        private InteractorCharacterProductsCarrier _carrier;
        
        private SellHelperTask _sellHelperTask;

        private CuttableItem[] _cuttableItems;
        public CuttingModule cuttingModule => _view.GetModule<CuttingModule>();

        private List<SellProduct> _usedProducts = new List<SellProduct>();
        
        private HelperTasksGroup _tasksGroup;

        public void Initialize(HelperView view, InteractorCharacterProductsCarrier carrier,  CuttableItem[] cuttableItems, SellHelperTask sellHelperTask)
        {
            _view = view;
            _carrier = carrier;
            _cuttableItems = cuttableItems;
            _sellHelperTask = sellHelperTask;
            
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }

        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                var product = GetReadyProduct();
                if(product == null) break;
                _usedProducts.Add(product);
                yield return null;
                Vector3 pos = product.transform.position;
                pos.y = 0;
                _view.taskModule.MoveTo(product.name, pos);
            
                yield return null;

                float timer = 0f;
                while (!_view.taskModule.aiPath.reachedDestination && _carrier.HasSpace() &&
                       product.interactItem.enabled && timer < maxTimeMoveToPoint)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
                _view.locomotionMovingModule.StopMovement();
                if (!_carrier.HasSpace())
                {
                    _tasksGroup.RunTask(_sellHelperTask);
                    while (_sellHelperTask.isRunning) yield return null;
                }
            }
            
            if (!_carrier.IsEmpty())
            {
                _tasksGroup.RunTask(_sellHelperTask);
                while (_sellHelperTask.isRunning) yield return null;
            }
        }

        private SellProduct GetReadyProduct()
        {
            foreach (var item in _cuttableItems)
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