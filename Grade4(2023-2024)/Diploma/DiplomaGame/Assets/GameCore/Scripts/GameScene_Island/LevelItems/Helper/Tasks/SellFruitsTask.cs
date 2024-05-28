using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Helper.Tasks
{
    public class SellFruitsTask : HelperTask
    {
        private HelperView _view;
        private InteractorCharacterProductsCarrier _carrier;
        private FruitTreeItem _fruitTreeItem;

        private List<SellProduct> _usedProducts = new List<SellProduct>();

        private SellHelperTask _sellHelperTask;
        
        private HelperTasksGroup _tasksGroup;
        
        public void Initialize(HelperView view, InteractorCharacterProductsCarrier carrier, FruitTreeItem fruitTreeItem, SellHelperTask sellHelperTask)
        {
            _view = view;
            _carrier = carrier;
            _fruitTreeItem = fruitTreeItem;
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
            if (_fruitTreeItem.fruitsAvailableForPickUp.Count <= 0) return null;

            return _fruitTreeItem.fruitsAvailableForPickUp.FirstOrDefault((prod) =>
                prod.interactItem.enabled && !_usedProducts.Contains(prod));
        }

        protected override void StopInternal()
        {
            base.StopInternal();
            _tasksGroup.StopTask();
            _usedProducts.Clear();
        }
    }
}