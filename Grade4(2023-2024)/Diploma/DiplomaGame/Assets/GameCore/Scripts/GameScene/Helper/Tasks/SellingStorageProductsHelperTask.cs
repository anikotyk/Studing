using System.Collections;
using System.Collections.Generic;
using GameCore.GameScene.Helper.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;

namespace GameCore.GameScene.Helper.Tasks
{
    public class SellingStorageProductsHelperTask : HelperTask
    {
        private HelperView _view;
        private InteractorCharacterProductsCarrier _carrier;
        
        private SellHelperTask _sellHelperTask;
        private List<GetProductFromStorageHelperTask> _getFishHelperTasks = new List<GetProductFromStorageHelperTask>();
        
        private HelperTasksGroup _tasksGroup;
        
        public void Initialize(HelperView view, InteractorCharacterProductsCarrier carrier, SellHelperTask sellHelperTask, List<LimitedProductStorage> storages)
        {
            _view = view;
            _carrier = carrier;
            _sellHelperTask = sellHelperTask;
            foreach (var storage in storages)
            {
                GetProductFromStorageHelperTask task = new GetProductFromStorageHelperTask();
                task.Initialize(view, carrier, storage);
                _getFishHelperTasks.Add(task);
            }
            
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }

        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                var task = GetGetFishTask();
                if(task == null) break;
                _tasksGroup.RunTask(task);
                while (task.isRunning) yield return null;
                
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

        private GetProductFromStorageHelperTask GetGetFishTask()
        {
            foreach (var task in _getFishHelperTasks)
            {
                if (task.storage.Has()) return task;
            }

            return null;
        }
        
        protected override void StopInternal()
        {
            base.StopInternal();
            _tasksGroup.StopTask();
        }
    }
}