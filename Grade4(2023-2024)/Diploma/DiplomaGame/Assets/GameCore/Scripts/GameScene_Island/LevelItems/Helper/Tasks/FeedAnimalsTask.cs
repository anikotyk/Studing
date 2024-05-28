using System.Collections;
using GameCore.GameScene_Island.LevelItems.Platforms;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Helper.Tasks
{
    public class FeedAnimalsTask : HelperTask
    {
        private HelperView _view;
        private InteractorCharacterProductsCarrier _carrier;
        
        private FeedPlatform[] _feedPlatformsCached;

        public FeedPlatform[] feedPlatforms
        {
            get
            {
                if (_feedPlatformsCached == null) _feedPlatformsCached = GameObject.FindObjectsOfType<FeedPlatform>(true);
                return _feedPlatformsCached;
            }
        }
        
        private CutItemsTask _cutItemsTask;
        private GetCutItemsTask _getCutItemsTask;
        private SellHelperTask _sellHelperTask;
        private HelperTasksGroup _tasksGroup;
        
        public void Initialize(HelperView view, InteractorCharacterProductsCarrier carrier, CutItemsTask cutItemsTask, GetCutItemsTask getCutItemsTask, SellHelperTask sellHelperTask)
        {
            _view = view;
            _carrier = carrier;
            _cutItemsTask = cutItemsTask;
            _getCutItemsTask = getCutItemsTask;
            _sellHelperTask = sellHelperTask;
            
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }
        
        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                var platform = GetFeedPlatform();
                if(platform == null) break;
                yield return null;

                if (_carrier.Count(platform.productDataConfig, false) <= 0)
                {
                    _tasksGroup.RunTask(_cutItemsTask);
                    while (_cutItemsTask.isRunning && _carrier.HasSpace() && platform.IsHasSpace()) yield return null;
                    _tasksGroup.StopTask();
                    if (_carrier.HasSpace() && platform.IsHasSpace())
                    {
                        _tasksGroup.RunTask(_getCutItemsTask);
                        while (_getCutItemsTask.isRunning && platform.IsHasSpace()) yield return null;
                        _tasksGroup.StopTask();
                    }
                   
                    if (_carrier.Count(platform.productDataConfig, false) <= 0)
                    {
                        _tasksGroup.RunTask(_sellHelperTask);
                        while (_sellHelperTask.isRunning) yield return null;
                        continue;
                    }
                }
                
                if(!platform.IsHasSpace()) continue;

                Vector3 pos = platform.transform.position;
                _view.taskModule.MoveTo(platform.name, pos);
            
                yield return null;
                float timer = 0f;
                while (!_view.taskModule.aiPath.reachedDestination && platform.IsHasSpace() && timer < maxTimeMoveToPoint)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
                _view.locomotionMovingModule.StopMovement();

                if (_view.taskModule.aiPath.reachedDestination && platform.IsHasSpace())
                {
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }

        private FeedPlatform GetFeedPlatform()
        {
            foreach (var platform in feedPlatforms)
            {
                if (platform.IsHasSpace()) return platform;
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