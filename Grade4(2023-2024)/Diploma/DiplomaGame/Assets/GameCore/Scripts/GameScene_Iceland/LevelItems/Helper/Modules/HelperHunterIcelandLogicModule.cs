using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.GameScene_Iceland.LevelItems.Managers;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Helper.Modules
{
    public class HelperHunterIcelandLogicModule : HelperLogicModule
    {
        [Inject, UsedImplicitly] public FightAnimalsManager fightAnimalsManager { get; }
        
        [SerializeField] private BuyObject _objectTurnOnHelper;
        [SerializeField] private ArriveInuitsCutscene _arriveInuitsCutscene;
        [SerializeField] private float _delayStartWorkAfterBuy;
        [SerializeField] private int _helperHunterIndex;

        private HitAnimalHelperTask _hitAnimalsTask;
        private SellHitItemsTask _sellHitItemsTask;

        protected override void Initialize()
        {
            base.Initialize();
            
            if (!_objectTurnOnHelper.isBought)
            {
                TurnOffHelper();
                _arriveInuitsCutscene.onFakeInuitsDeactivated.Once(() =>
                {
                    AstarPath.active.Scan();
                    view.gameObject.SetActive(true);
                    view.sellStorageModule.productsStorage.SetActive(true);
                    DOVirtual.DelayedCall(_delayStartWorkAfterBuy, ValidateHelper, false).SetLink(gameObject);
                });
            }
            else
            {
                view.sellStorageModule.productsStorage.SetActive(true);
                ValidateHelper();
            }

            if (!_objectTurnOnHelper.isBought)
            {
                view.gameObject.SetActive(false);
                view.sellStorageModule.productsStorage.SetActive(false);
            }
        }

        protected override void SetUpTasks()
        {
            base.SetUpTasks();
            
            _tasksGroup = new HelperTasksQueueGroup();
            _tasksGroup.Initialize(view, restTask);

            SetSellHitItemsTask();
            SetUpHitAnimalsTask();
        }

        private void SetSellHitItemsTask()
        {
            _sellHitItemsTask = new SellHitItemsTask();
            _sellHitItemsTask.Initialize(view, carrier, sellTask);
        }

        private void SetUpHitAnimalsTask()
        {
            _hitAnimalsTask = new HitAnimalHelperTask();
            _hitAnimalsTask.Initialize(view, fightAnimalsManager.animals, _helperHunterIndex, _sellHitItemsTask);

            foreach (var animal in fightAnimalsManager.animals)
            {
                animal.onActivate.On(() =>
                {
                    if (!_hitAnimalsTask.isRunning)
                    {
                        _tasksGroup.RunTask(_hitAnimalsTask);
                    }
                });
                if (animal.isActive) _tasksGroup.RunTask(_hitAnimalsTask);
            }
        }
    }
}