using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Character.CharacterTyping;
using GameCore.Common.Misc;
using GameCore.Common.Saves;
using GameCore.Common.UI;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Helper.Tasks;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameCore.GameScene.LevelItems;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Factories;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Helper.Modules
{
    public class HelperIslandLogicModule : HelperLogicModule
    {
        [Inject, UsedImplicitly] public CharacterTypeSaveData characterTypeSaveData { get; }
        [Inject, UsedImplicitly] public VfxStack vfxStack { get; }
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        
        [SerializeField] private BuyObject _objectTurnOnHelper;
        [SerializeField] private Transform _appearPoint;
        [SerializeField] private AudioSource _appearLongSound;
        [SerializeField] private AudioSource _appearShortSound;
        [SerializeField] private WaterFilterObject _waterFilterObject;
        [SerializeField] private FruitTreeItem _fruitTreeItem;
        
        [SerializeField] private Sprite _helperGirlSprite;
        [SerializeField] private Sprite _helperBoySprite;
        [SerializeField] private string _helperAppearText;
        
        private SellingStorageProductsHelperTask _sellingStorageProductsTask;
        private WateringHelperTask _wateringTask;
        private HitFruitTreeTask _hitFruitTreeTask;
        private SellFruitsTask _sellFruitsTask;
        private CutItemsTask _cutItemsTask;
        private SellCutItemsTask _sellCutItemsTask;

        protected override void Initialize()
        {
            base.Initialize();

            if (!_objectTurnOnHelper.isBought)
            {
                view.gameObject.SetActive(false);
                view.sellStorageModule.productsStorage.SetActive(false);
                _objectTurnOnHelper.onBuy.Once(CutsceneBuyHelper);
                return;
            }
            
            ValidateHelper();
        }

        private void CutsceneBuyHelper()
        {
            AstarPath.active.Scan();
            
            view.transform.position = _appearPoint.position;
            
            helperAppearVCam.gameObject.SetActive(true);
            gameplayUIMenuWindow.Hide();
            menuBlockOverlay.Activate(this);
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);

            DOVirtual.DelayedCall(0.75f, () =>
            {
                vfxStack.Spawn(CommStr.AppearHelperVFX, view.transform.position + Vector3.up * 0.5f);
                _appearLongSound.Play();
                
                DOVirtual.DelayedCall(1.7f, () =>
                {  
                    _appearShortSound.Play();
                    
                    view.gameObject.SetActive(true);
                    view.sellStorageModule.productsStorage.SetActive(true);
                    view.transform.localScale = Vector3.one * 0.01f;
                    view.transform.DOScale(Vector3.one, 0.45f).SetEase(Ease.OutBack).SetLink(gameObject);
                    
                    DOVirtual.DelayedCall(0.75f, () =>
                    {  
                        
                        windowFactory.Create<CharacterReplicaDialog>(CommStr.CharacterReplicaDialog, window =>
                        {
                            var sprite = characterTypeSaveData.value.type == CharacterType.Type.boy ? _helperGirlSprite : _helperBoySprite;
                            window.Initialize(sprite, _helperAppearText);
                            window.Show();
                            window.onCloseClick.Once(() =>
                            {
                                helperAppearVCam.gameObject.SetActive(false);
                                
                                DOVirtual.DelayedCall(1f, () =>
                                {
                                    gameplayUIMenuWindow.Show();
                                    menuBlockOverlay.Deactivate(this);
                                    popUpsController.containerUnderMenu.gameObject.SetActive(true);
                                    popUpsController.containerOverWindow.gameObject.SetActive(true);
                        
                                    ValidateHelper();
                                },false).SetLink(gameObject);
                            });
                        }, false);
                    }, false).SetLink(gameObject);
                }, false).SetLink(gameObject);
            }, false).SetLink(gameObject);
        }

        protected override void SetUpTasks()
        {
            base.SetUpTasks();
            
            _tasksGroup = new HelperTasksQueueGroup();
            _tasksGroup.Initialize(view, restTask);

            SetUpWateringTask();
            SetUpFishSellingTask();
            SetUpHitAppleTreeTask();
            SetUpSellApplesTask();
            SetUpCutItemsTask();
            SetUpSellCutItemsTask();
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

        private void SetUpWateringTask()
        {
            _wateringTask = new WateringHelperTask();
            _wateringTask.Initialize(view, _waterFilterObject);

            _waterFilterObject.onNeedsWaterAndEnabled.On(() =>
            {
                _tasksGroup.RunTask(_wateringTask);
            });
            
            if (_waterFilterObject.isNeedWater)
            {
                _tasksGroup.RunTask(_wateringTask);
            }
        }
        
        private void SetUpHitAppleTreeTask()
        {
            _hitFruitTreeTask = new HitFruitTreeTask();
            _hitFruitTreeTask.Initialize(view, _fruitTreeItem);

            _fruitTreeItem.onEnabled.On(() =>
            {
                _tasksGroup.RunTask(_hitFruitTreeTask);
            });

            if (_fruitTreeItem.isEnabled)
            {
                _tasksGroup.RunTask(_hitFruitTreeTask);
            }
        }
        
        private void SetUpSellApplesTask()
        {
            _sellFruitsTask = new SellFruitsTask();
            _sellFruitsTask.Initialize(view, carrier, _fruitTreeItem, sellTask);

            _fruitTreeItem.onFruitsReadyToCollect.On(() =>
            {
                _tasksGroup.RunTask(_sellFruitsTask);
            });

            if (_fruitTreeItem.fruitsAvailableForPickUp.Count > 0)
            {
                _tasksGroup.RunTask(_sellFruitsTask);
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
    }
}