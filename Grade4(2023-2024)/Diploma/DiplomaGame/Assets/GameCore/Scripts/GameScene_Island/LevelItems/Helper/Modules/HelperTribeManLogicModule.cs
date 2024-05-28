using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Helper.Tasks;
using GameCore.GameScene_Island.LevelItems.Managers;
using GameCore.GameScene_Island.LevelItems.Platforms;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameCore.GameScene.LevelItems;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Helper.Modules
{
    public class HelperTribeManLogicModule : HelperLogicModule
    {
        [SerializeField] private HelperTribeManCutsceneManager _helperTribeManCutsceneManager;
        [SerializeField] private GameObject _feedPlace;
        [SerializeField] private AnimatorParameterApplier _restHungryAnim;
        [SerializeField] private AnimatorParameterApplier _endRestHungryAnim;
        [SerializeField] private BuyObject _objectTurnOnHelper;
        [SerializeField] private Transform _appearPoint;
        [SerializeField] private FruitTreeItem _fruitTreeItem;
        [SerializeField] private WaterFilterObject _waterFilterObject;
        [SerializeField] private ParticleSystem _vfxHungry;
        [SerializeField] private SellProductsCollectPlatform _millWheatCollectPlatform;
        [SerializeField] private SellProductsCollectPlatform _helperEatCollectPlatform;
        [SerializeField] private ProductDataConfig _wheatConfig;
        [SerializeField] private List<LimitedProductStorage> _flourStorages;
        [SerializeField] private AnimatorParameterApplier _eatAnim;
        [SerializeField] private Transform _eatPoint;

        public override string appearVCamConfig => GameplaySettings.def.helperTribeManAppearVCamIdConfig.id;
        
        private HitFruitTreeTask _hitFruitTreeTask;
        private SellFruitsTask _sellFruitsTask;
        
        private WateringHelperTask _wateringTask;
        
        private CutItemsTask _cutItemsTask;
        private GetCutItemsTask _getCutItemsTask;
        private CutItemsTask _cutNotWheatItemsTask;
        private SellCutItemsTask _sellCutItemsTask;
        
        private MakeFlourTask _makeFlourTask;
        private SellingStorageProductsHelperTask _sellingStorageProductsTask;
        
        private HelperEatTask _helperEatTask;
        private HungryRestHelperTask _hungryRestTask;

        private HelperTasksQueueGroup _eatTasksGroup;
        private Tween _hungryTween;
        private bool _isHungry;

        protected override void Initialize()
        {
            base.Initialize();

            if (!_objectTurnOnHelper.isBought)
            {
                view.gameObject.SetActive(false);
                view.sellStorageModule.productsStorage.gameObject.SetActive(false);
                _feedPlace.gameObject.SetActive(false);
                _objectTurnOnHelper.onBuy.Once(CutsceneBuyHelper);
                return;
            }

            ValidateHelper();
        }

        private void CutsceneBuyHelper()
        {
            _helperTribeManCutsceneManager.StartCutscene();
            
            _helperTribeManCutsceneManager.onCutsceneEnded.Once(() =>
            {
                view.transform.position = _appearPoint.position;
                view.gameObject.SetActive(true);
                view.sellStorageModule.productsStorage.SetActive(true);
                _feedPlace.gameObject.SetActive(true);
                ValidateHelper();
                SetHungry();
            });
        }

        protected override void SetUpTasks()
        {
            base.SetUpTasks();  

            _tasksGroup = new HelperTasksQueueGroup();
            _tasksGroup.Initialize(view, restTask);

            SetUpCutItemsTask();
            SetUpGetCutItemsTask();
            SetUpHitFruitTreeTask();
            SetUpSellFruitsTask();
            SetUpWateringTask();
            SetUpMakeFlourTask();
            SetUpEatTask();
            SetUpCutNotWheatTask();
            SetUpSellCutItemsTask();
            SetUpFlourSellingTask();
        }

        private void SetUpHitFruitTreeTask()
        {
            _hitFruitTreeTask = new HitFruitTreeTask();
            _hitFruitTreeTask.Initialize(view, _fruitTreeItem);

            _fruitTreeItem.onEnabled.On(() => { _tasksGroup.RunTask(_hitFruitTreeTask); });

            if (_fruitTreeItem.isEnabled)
            {
                _tasksGroup.RunTask(_hitFruitTreeTask);
            }
        }

        private void SetUpCutItemsTask()
        {
            _cutItemsTask = new CutItemsTask();
            _cutItemsTask.Initialize(view);
        }

        private void SetUpMakeFlourTask()
        {
            _makeFlourTask = new MakeFlourTask();
            _makeFlourTask.Initialize(view, carrier, _cutItemsTask, _getCutItemsTask, sellTask,
                _millWheatCollectPlatform);

            var cuttableItems = GameObject.FindObjectsOfType<CuttableItem>(true).ToList()
                .FindAll(item => item.spawnProductConfig.id == _wheatConfig.id).ToArray();

            foreach (var item in cuttableItems)
            {
                item.onEnabled.On(() => { _tasksGroup.RunTask(_makeFlourTask); });
            }

            if (cuttableItems.FirstOrDefault(item => item.interactItem.enabled) != null)
            {
                _tasksGroup.RunTask(_makeFlourTask);
            }
        }

        private void SetUpSellFruitsTask()
        {
            _sellFruitsTask = new SellFruitsTask();
            _sellFruitsTask.Initialize(view, carrier, _fruitTreeItem, sellTask);

            _fruitTreeItem.onFruitsReadyToCollect.On(() => { _tasksGroup.RunTask(_sellFruitsTask); });

            if (_fruitTreeItem.fruitsAvailableForPickUp.Count > 0)
            {
                _tasksGroup.RunTask(_sellFruitsTask);
            }
        }
        
        private void SetUpCutNotWheatTask()
        {
            _cutNotWheatItemsTask = new CutItemsTask();
            _cutNotWheatItemsTask.Initialize(view);

            var cuttableItems = GameObject.FindObjectsOfType<CuttableItem>(true).ToList().FindAll(item => 
                view.GetModule<CuttingModule>().IsAbleToCut(item.spawnProductConfig) && item.spawnProductConfig.id != _wheatConfig.id).ToArray();
            foreach (var item in cuttableItems)
            {
                item.onEnabled.On(() =>
                {
                    _tasksGroup.RunTask(_cutNotWheatItemsTask);
                });
            }

            if (cuttableItems.FirstOrDefault(item => item.interactItem.enabled) != null)
            {
                _tasksGroup.RunTask(_cutItemsTask);
            }
        }
        
        private void SetUpSellCutItemsTask()
        {
            var cuttableItems = GameObject.FindObjectsOfType<CuttableItem>(true).ToList().FindAll(item => 
                view.GetModule<CuttingModule>().IsAbleToCut(item.spawnProductConfig) && item.spawnProductConfig.id != _wheatConfig.id).ToArray();

            
            _sellCutItemsTask = new SellCutItemsTask();
            _sellCutItemsTask.Initialize(view, carrier, cuttableItems, sellTask);
            
            foreach (var item in cuttableItems)
            {
                item.onSpawnedProducts.On(() =>
                {
                    _tasksGroup.RunTask(_sellCutItemsTask);
                });
            }
            
            foreach (var item in cuttableItems)
            {
                if(item.productsAvailableForPickUp.Count > 0)
                {
                    _tasksGroup.RunTask(_sellCutItemsTask);
                    break;
                }
            }
        }

        private void SetUpGetCutItemsTask()
        {
            _getCutItemsTask = new GetCutItemsTask();
            _getCutItemsTask.Initialize(view, carrier);
        }

        private void SetUpWateringTask()
        {
            _wateringTask = new WateringHelperTask();
            _wateringTask.Initialize(view, _waterFilterObject);

            _waterFilterObject.onNeedsWaterAndEnabled.On(() => { _tasksGroup.RunTask(_wateringTask); });

            if (_waterFilterObject.isNeedWater)
            {
                _tasksGroup.RunTask(_wateringTask);
            }
        }
        
        private void SetUpFlourSellingTask()
        {
            _sellingStorageProductsTask = new SellingStorageProductsHelperTask();
            _sellingStorageProductsTask.Initialize(view, carrier, sellTask, _flourStorages);
            
            foreach (var storage in _flourStorages)
            {
                storage.onChange.On(() =>
                {
                    if (storage.Has())
                    {
                        _tasksGroup.RunTask(_sellingStorageProductsTask);
                    }
                });
            }
            
            if (_flourStorages.FirstOrDefault(storage => storage.Has()))
            {
                _tasksGroup.RunTask(_sellingStorageProductsTask);
            }
        }


        private void SetUpEatTask()
        {
            _helperEatTask = new HelperEatTask();
            _helperEatTask.Initialize(view, _eatPoint, _eatAnim, _helperEatCollectPlatform, sellTask);
            
            _hungryRestTask = new HungryRestHelperTask();
            _hungryRestTask.Initialize(view, _restHungryAnim, _endRestHungryAnim, sellTask, _vfxHungry);

            _eatTasksGroup = new HelperTasksQueueGroup();
            _eatTasksGroup.Initialize(view, _hungryRestTask);

            SetHungryTween();

            _helperEatTask.onEat.On(() =>
            {
                _isHungry = false;
                _vfxHungry.Stop();
                _eatTasksGroup.DisableTaskGroup();
                _tasksGroup.EnableTaskGroup();
                _tasksGroup.RestartTask();
                SetHungryTween();
            });

            _helperEatCollectPlatform.productsCarrier.onChange.On(() =>
            {
                if (_helperEatCollectPlatform.productsCarrier.count > 0 && _isHungry)
                {
                    _eatTasksGroup.RunTaskAndCancelOther(_helperEatTask);
                    _vfxHungry.Stop();
                }
            });
        }
        

        private void SetHungryTween()
        {
            if (_hungryTween != null)
            {
                _hungryTween.Kill();
            }
            
            _hungryTween = DOVirtual.DelayedCall(GameplaySettings.def.tribeManWorkingTime, () =>
            {
                SetHungry();
            }, false).SetLink(gameObject);
        }

        private void SetHungry()
        {
            if (_hungryTween != null)
            {
                _hungryTween.Kill();
            }
            
            _tasksGroup.DisableTaskGroup();
            _eatTasksGroup.EnableTaskGroup();

            if (_helperEatCollectPlatform.productsCarrier.count > 0)
            {
                _eatTasksGroup.RunTaskAndCancelOther(_helperEatTask);
            }
            else
            {
                _isHungry = true;
                _eatTasksGroup.RestartTask();
            }
        }
    }
}