using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Animal.Tasks;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class AnimalProductingAutomaticallyLogicModule : AnimalProductingLogicModule
    {
        [SerializeField] private Transform _eatPoint;
        [SerializeField] private float _maxDelayHungry = 30f;
        
        private AnimalEatAutomaticallyTask _eatTask;
        
        protected override void SetUpEatTask()
        {
            void StartEatTaskWithDelay()
            {
                float randomDelay = Random.Range(0, _maxDelayHungry);
                DOVirtual.DelayedCall(randomDelay, () =>
                {
                    tasksGroup.RunTask(_eatTask);
                }, false).SetLink(gameObject);
            }
            
            _eatTask = new AnimalEatAutomaticallyTask();
            _eatTask.Initialize(view, _eatPoint);
            
            view.productionModule.onBecomeHungry.On(() =>
            {
                StartEatTaskWithDelay();
            });

            StartEatTaskWithDelay();
        }
    }
}