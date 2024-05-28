using System.Collections;
using System.Collections.Generic;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene.Helper.Tasks
{
    public class SellHitItemsTask : HelperTask
    {
        private HelperView _view;
        private InteractorCharacterProductsCarrier _carrier;

        private SellHelperTask _sellTask;
        
        private HelperTasksGroup _tasksGroup;
        private List<SellProduct> _hitProducts = new List<SellProduct>(); 
        private List<SellProduct> _usedProducts = new List<SellProduct>();

        public void Initialize(HelperView view, InteractorCharacterProductsCarrier carrier, SellHelperTask sellTask)
        {
            _view = view;
            _carrier = carrier;
            
            _sellTask = sellTask;
            
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }

        public void SetHitProducts(List<SellProduct> hitProducts)
        {
            _hitProducts = hitProducts;
        }

        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                if (!_carrier.HasSpace())
                {
                    _tasksGroup.RunTask(_sellTask);
                    while (_sellTask.isRunning) yield return null;
                }
                
                var product = GetProduct();
                if(product == null) break;
                _usedProducts.Add(product);
                yield return null;
                Vector3 pos = product.transform.position;
                pos.y = 0;
                _view.taskModule.MoveTo(product.name, pos);
            
                yield return null;
                float timer = 0;
                while (!_view.taskModule.aiPath.reachedDestination && !product.isInCarrier && _carrier.HasSpace() && timer < maxTimeMoveToPoint)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
                _view.locomotionMovingModule.StopMovement();
                yield return null;
                if (!_carrier.HasSpace() && !product.isInCarrier) _usedProducts.Remove(product);
            }
            yield return new WaitForSeconds(0.5f);
            if (!_carrier.IsEmpty())
            {
                _tasksGroup.RunTask(_sellTask);
                yield return null;
                while (_sellTask.isRunning) yield return null;
            }
        }

        private SellProduct GetProduct()
        {
            foreach (var prod in _hitProducts)
            {
                if (prod.interactItem.enabled && !prod.isInCarrier && !_usedProducts.Contains(prod))
                {
                    return prod;
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