using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Helper.Tasks;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.Helper.Tasks;
using GameCore.GameScene.LevelItems;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.Helper.Modules
{
    public class HelperRaftLogicModule : HelperLogicModule
    {
        [SerializeField] private GameObject _vfxOnAppear;
        [SerializeField] private Transform _defaultPoint2;
        [SerializeField] private BuyObject _buyObject;
        [SerializeField] private BuyObject _buyObjectWorkbench;
        [SerializeField] private BuyObject _buyObjectActivate;
        [SerializeField] private EventObtainObject _eventObtainObject;
        [SerializeField] private BuyObject _buyObjectSwitchDefaultPoint;
        [SerializeField] private WaterFilterObject _waterFilterObject;
        
        private bool _isSwitchDefaultPoint;
        public override Transform defaultPoint => _isSwitchDefaultPoint ? _defaultPoint2 : _defaultPoint;

        private CinemachineVirtualCamera _workbenchVCamCached;
        public CinemachineVirtualCamera workbenchVCam
        {
            get
            {
                if (_workbenchVCamCached == null)
                {
                    _workbenchVCamCached =  virtualCameraLinks.First(v => v.id == GameplaySettings.def.workbenchVCamIdConfig.id).virtualCamera;
                }
                return _workbenchVCamCached;
            }
        }
        
        private SellingStorageProductsHelperTask _sellingStorageProductsTask;
        private CutItemsTask _cutItemsTask;
        private SellCutItemsTask _sellCutItemsTask;
        private WateringHelperTask _wateringTask;

        protected override void Initialize()
        {
            base.Initialize();

            CheckDefaultPoint();

            if (_buyObject.isBought)
            {
                ValidateHelper();
            }
            else if (_buyObjectActivate.isBought || _buyObjectActivate.isInBuyMode)
            {
                SetInNotBoughtMode();
                
                if (_buyObjectActivate.isInBuyMode)
                {
                    _buyObjectActivate.Buy();
                    _buyObjectWorkbench.Buy();
                }
            }
            else
            {
                view.gameObject.SetActive(false);

                _buyObjectActivate.onSetInBuyMode.Once(() =>
                {
                    _buyObjectActivate.Buy();
                    
                    SetInNotBoughtMode();
                    OnActivateInNotBoughtMode();
                });
            }
        }

        private void OnActivateInNotBoughtMode()
        {
            view.gameObject.SetActive(true);
            view.transform.localScale = Vector3.one * 0.01f;
            view.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                helperAppearVCam.gameObject.SetActive(true);
                _vfxOnAppear.SetActive(true);
                gameplayUIMenuWindow.Hide();
                menuBlockOverlay.Activate(this);
                popUpsController.containerUnderMenu.gameObject.SetActive(false);
                popUpsController.containerOverWindow.gameObject.SetActive(false);

                DOVirtual.DelayedCall(3f, () =>
                {
                    helperAppearVCam.gameObject.SetActive(false);
                    workbenchVCam.gameObject.SetActive(true);
                    DOVirtual.DelayedCall(2f, () =>
                    {
                        popUpsController.containerUnderMenu.gameObject.SetActive(true);
                        popUpsController.containerOverWindow.gameObject.SetActive(true);
                                
                        _buyObjectWorkbench.Buy();
                        DOVirtual.DelayedCall(1.5f, () =>
                        {
                            workbenchVCam.gameObject.SetActive(false);
                                    
                            gameplayUIMenuWindow.Show();
                            menuBlockOverlay.Deactivate(this);
                        },false).SetLink(gameObject);
                    },false).SetLink(gameObject);
                },false).SetLink(gameObject);
            },false).SetLink(gameObject);
        }

        private void SetInNotBoughtMode()
        {
            view.GetModule<HelperNotBoughtModule>().ActivateInternal();
            
            _eventObtainObject.onClosedObtainDialog.Once(() =>
            {
                view.GetModule<HelperNotBoughtModule>().OnBuy();
            });
            
            view.GetModule<HelperNotBoughtModule>().onBuyCutsceneEnded.Once(ValidateHelper);
        }
        
        protected override void SetUpTasks()
        {
            base.SetUpTasks();
            
            _tasksGroup = new HelperTasksQueueGroup();
            _tasksGroup.Initialize(view, restTask);
            
            SetUpWateringTask();
            SetUpFishSellingTask();
            SetUpCutItemsTask();
            SetUpSellCutItemsTask();
        }

        private void CheckDefaultPoint()
        {
            DOVirtual.DelayedCall(0.1f, ()=>
            {
                if (_buyObjectSwitchDefaultPoint.isBought)
                {
                    _isSwitchDefaultPoint = true;
                    view.transform.position = defaultPoint.position;
                }
            }, false).SetLink(gameObject);
            
            _buyObjectSwitchDefaultPoint.onBuy.Once(() =>
            {
                _isSwitchDefaultPoint = true;
            });
        }
        
        private void SetUpFishSellingTask()
        {
            List<LimitedProductStorage> storages = GameObject.FindObjectsOfType<FishStorage>(true).Select(fishStorage => fishStorage.storage).ToList();
            
            _sellingStorageProductsTask = new SellingStorageProductsHelperTask();
            _sellingStorageProductsTask.Initialize(view, carrier, sellTask, storages);
            
            foreach (var storage in storages)
            {
                storage.onChange.On(() =>
                {
                    if (storage.Has())
                    {
                        _tasksGroup.RunTask(_sellingStorageProductsTask);
                    }
                });
            }
            
            if (storages.FirstOrDefault(storage => storage.Has()))
            {
                _tasksGroup.RunTask(_sellingStorageProductsTask);
            }

        }
        
        private void SetUpCutItemsTask()
        {
            _cutItemsTask = new CutItemsTask();
            _cutItemsTask.Initialize(view);

            var cuttableItems = GameObject.FindObjectsOfType<CuttableItem>(true).ToList().FindAll(item => view.GetModule<CuttingModule>().IsAbleToCut(item.spawnProductConfig)).ToArray();
            foreach (var item in cuttableItems)
            {
                item.onEnabled.On(() =>
                {
                    _tasksGroup.RunTask(_cutItemsTask);
                });
            }

            if (cuttableItems.FirstOrDefault(item => item.interactItem.enabled) != null)
            {
                _tasksGroup.RunTask(_cutItemsTask);
            }
        }
        
        private void SetUpSellCutItemsTask()
        {
            var cuttableItems = GameObject.FindObjectsOfType<CuttableItem>(true).ToList().FindAll(item => view.GetModule<CuttingModule>().IsAbleToCut(item.spawnProductConfig)).ToArray();
            
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
        
        private void SetUpWateringTask()
        {
            _wateringTask = new WateringHelperTask();
            _wateringTask.Initialize(view, _waterFilterObject);

            _waterFilterObject.onNeedsWaterAndEnabled.On(() =>
            {
                _tasksGroup.RunTask(_wateringTask);
            });
            
            if (_waterFilterObject.isNeedWater && _waterFilterObject.isEnabled)
            {
                _tasksGroup.RunTask(_wateringTask);
            }
        }
    }
}