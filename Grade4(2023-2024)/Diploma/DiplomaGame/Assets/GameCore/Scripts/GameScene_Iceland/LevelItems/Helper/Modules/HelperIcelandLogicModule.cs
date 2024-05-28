using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Items.HittableItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Helper.Modules
{
    public class HelperIcelandLogicModule : HelperLogicModule
    {
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        
        [SerializeField] private BuyObject _objectTurnOnHelper;
        [SerializeField] private BuyObject _objectNotColdHelper;
        [SerializeField] private BuyObject _objectSpeedUpHelper;
        [SerializeField] private float _speedMultipler = 1.15f;
        
        private HitHelperTask _hitTask;
        private SellHitItemsTask _sellHitItemsTask;

        protected override void Initialize()
        {
            base.Initialize();

            _objectTurnOnHelper.onBuy.Once(CutsceneBuyHelper);
            _objectNotColdHelper.onBuy.Once(()=>
            {
                view.GetModule<HelperColdModule>().SetNotCold();
                ValidateHelper();
            });
            _objectSpeedUpHelper.onBuy.Once(() =>
            {
                view.GetModule<HelperSpeedModule>().SetSpeedMultiplier(_speedMultipler);
            });
            
            if (!_objectNotColdHelper.isBought)
            {
                TurnOffHelper();
            }
            else
            {
                ValidateHelper();
            }

            if (!_objectTurnOnHelper.isBought)
            {
                view.gameObject.SetActive(false);
                view.sellStorageModule.productsStorage.SetActive(false);
            }else if (!_objectNotColdHelper.isBought)
            {
                view.GetModule<HelperColdModule>().SetCold();
            }else if (_objectSpeedUpHelper.isBought)
            {
                view.GetModule<HelperSpeedModule>().SetSpeedMultiplier(_speedMultipler);
            }
        }

        private void CutsceneBuyHelper()
        {
            AstarPath.active.Scan();
            view.gameObject.SetActive(true);
            view.sellStorageModule.productsStorage.SetActive(true);
            view.transform.localScale = Vector3.one * 0.01f;
            view.transform.DOScale(Vector3.one, 0.45f).SetEase(Ease.OutBack).SetLink(gameObject);
            
            view.GetModule<HelperColdModule>().SetCold();
        }

        protected override void SetUpTasks()
        {
            base.SetUpTasks();
            
            _tasksGroup = new HelperTasksQueueGroup();
            _tasksGroup.Initialize(view, restTask);

            SetSellHitItemsTask();
            SetUpHitTask();
        }

        private void SetSellHitItemsTask()
        {
            _sellHitItemsTask = new SellHitItemsTask();
            _sellHitItemsTask.Initialize(view, carrier, sellTask);
        }

        private void SetUpHitTask()
        {
            List<ProductingHittableItem> hittables = buyObjectsManager.gameObject.GetComponentsInChildren<ProductingHittableItem>(true).ToList();
            _hitTask = new HitHelperTask();
            _hitTask.Initialize(view, hittables, _sellHitItemsTask);

            foreach (var hittable in hittables)
            {
                hittable.onTurnOn.On(() =>
                {
                    if (!_hitTask.isRunning)
                    {
                        _tasksGroup.RunTask(_hitTask);
                    }
                });
            }
            view.sellStorageModule.storageCarrier.onChange.On(() =>
            {
                if (view.sellStorageModule.storageCarrier.HasSpace() && !_hitTask.isRunning) _tasksGroup.RunTask(_hitTask);
            });
            
            _tasksGroup.RunTask(_hitTask);
        }
    }
}