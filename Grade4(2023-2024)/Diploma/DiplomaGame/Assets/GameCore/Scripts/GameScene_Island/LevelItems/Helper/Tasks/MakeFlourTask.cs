using System.Collections;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene_Island.LevelItems.Platforms;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Helper.Tasks
{
    public class MakeFlourTask : HelperTask
    {
        private HelperView _view;
        
        private CutItemsTask _cutItemsTask;
        private GetCutItemsTask _getCutItemsTask;
        private SellHelperTask _sellHelperTask;
        private HelperTasksGroup _tasksGroup;
        private InteractorCharacterProductsCarrier _carrier;
        private SellProductsCollectPlatform _wheatCollectPlatform;
        
        private MillItem _millItemCached;
        public MillItem millItem
        {
            get
            {
                if (_millItemCached == null) _millItemCached = GameObject.FindObjectOfType<MillItem>();
                return _millItemCached;
            }
        }

        public void Initialize(HelperView view, InteractorCharacterProductsCarrier carrier, CutItemsTask cutItemsTask,GetCutItemsTask getCutItemsTask,  SellHelperTask sellHelperTask, 
            SellProductsCollectPlatform wheatCollectPlatform)
        {
            _view = view;
            _carrier = carrier;
            _cutItemsTask = cutItemsTask;
            _getCutItemsTask = getCutItemsTask;
            _sellHelperTask = sellHelperTask;
            _wheatCollectPlatform = wheatCollectPlatform;
            
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }

        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                if(!_cutItemsTask.CanRun()) yield break;
                
                if (_carrier.Count(_wheatCollectPlatform.productDataConfig, false) <= 0)
                {
                    _tasksGroup.RunTask(_cutItemsTask);
                    while (_cutItemsTask.isRunning && _carrier.HasSpace()) yield return null;
                    _tasksGroup.StopTask();
                    if (_carrier.HasSpace())
                    {
                        _tasksGroup.RunTask(_getCutItemsTask);
                        while (_getCutItemsTask.isRunning) yield return null;
                        _tasksGroup.StopTask();
                    }
                   
                    if (_carrier.Count(_wheatCollectPlatform.productDataConfig, false) <= 0)
                    {
                        _tasksGroup.RunTask(_sellHelperTask);
                        while (_sellHelperTask.isRunning) yield return null;
                        continue;
                    }
                }

                if (_wheatCollectPlatform.gameObject.activeInHierarchy && _wheatCollectPlatform.productsCarrier.HasSpace() && _wheatCollectPlatform.interactItem.enabled)
                {
                    _view.taskModule.MoveTo(_wheatCollectPlatform.name, _wheatCollectPlatform.transform.position);
                   
                    yield return null;

                    float timer = 0f;
                    while (!_view.taskModule.aiPath.reachedDestination &&
                           _wheatCollectPlatform.productsCarrier.HasSpace() &&
                           _wheatCollectPlatform.interactItem.enabled && timer < maxTimeMoveToPoint)
                    {
                        yield return null;
                        timer += Time.deltaTime;
                    }
                    _view.locomotionMovingModule.StopMovement();

                    if (_view.taskModule.aiPath.reachedDestination && _wheatCollectPlatform.productsCarrier.HasSpace() && _wheatCollectPlatform.interactItem.enabled)
                    {
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                
                if (_carrier.products.Count > 0)
                {
                    _tasksGroup.RunTask(_sellHelperTask);
                    while (_sellHelperTask.isRunning) yield return null;
                }
                
                if (_wheatCollectPlatform.gameObject.activeInHierarchy && millItem.interactableProductionItem.CanProduct())
                {
                    yield return _view.StartCoroutine(MillGrindingCoroutine());
                }
            }
        }

        private IEnumerator MillGrindingCoroutine()
        {
            yield return null;
            if (!millItem.isEnabled || _wheatCollectPlatform.productsCarrier.IsEmpty() || _view.GetModule<MillGrindingModule>().isRunning) yield break;
            yield return null;
            _view.taskModule.MoveTo("MillGrindPoint", millItem.interactPoint.position);
            yield return null;
            while (!_view.taskModule.aiPath.reachedDestination && millItem.isEnabled && !_wheatCollectPlatform.productsCarrier.IsEmpty()) yield return null;
            _view.locomotionMovingModule.StopMovement();
            yield return null;
            if((millItem.isEnabled && !_wheatCollectPlatform.productsCarrier.IsEmpty()) || _view.GetModule<MillGrindingModule>().isRunning)
            {
                yield return new WaitForSeconds(0.75f);
                if (!_view.GetModule<MillGrindingModule>().isRunning && millItem.isEnabled)
                {
                    _view.locomotionMovingModule.StopMovement();
                    yield return new WaitForSeconds(0.75f);
                }
                while (_view.GetModule<MillGrindingModule>().isRunning) yield return null;
            }
        }
        
        protected override void StopInternal()
        {
            base.StopInternal();
            _tasksGroup.StopTask();
        }
    }
}