using GameCore.GameScene_Island.LevelItems.Animal.Tasks;
using GameCore.GameScene_Island.LevelItems.Platforms;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class AnimalProductingFeedableLogicModule : AnimalProductingLogicModule
    {
        [SerializeField] private FeedPlatform _feedPlatform;
        [SerializeField] private ProductDataConfig _eatProductDC;
        
        private AnimalEatTask _eatTask;
        
        protected override void SetUpEatTask()
        {
            _eatTask = new AnimalEatTask();
            _eatTask.Initialize(view, _feedPlatform, _eatProductDC);
            
            _feedPlatform.onFullCarrier.On(() =>
            {
                if (view.productionModule.isHungry)
                {
                    tasksGroup.RunTask(_eatTask);
                }
            });

            view.productionModule.onBecomeHungry.On(() =>
            {
                if (!_feedPlatform.productsCarrier.HasSpace())
                {
                    tasksGroup.RunTask(_eatTask);
                }
            });
        }
    }
}