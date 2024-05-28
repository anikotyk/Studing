using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Animal;
using GameCore.GameScene_Island.LevelItems.Helper.Tasks;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameCore.GameScene.LevelItems;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Helper.Modules
{
    public class HelperSonIcelandLogicModule : HelperLogicModule
    {
        [Inject, UsedImplicitly] public ContainerAnimalProducts containerAnimalProducts { get; }
        
        [SerializeField] private BuyObject _objectTurnOnHelper;
        
        [SerializeField] private AnimalProductingView[] _animals;
        
        private InteractAnimalsTask _interactAnimalsTask;
        private SellAnimalsProductsTask _sellAnimalsProductsTask;
        private SellingStorageProductsHelperTask _sellingStorageProductsTask;

        protected override void Initialize()
        {
            base.Initialize();
            
            if (!_objectTurnOnHelper.isBought)
            {
                view.sellStorageModule.productsStorage.SetActive(false);
                view.gameObject.SetActive(false);
                _objectTurnOnHelper.onBuy.Once(CutsceneBuyHelper);
                return;
            }
            
            ValidateHelper();
        }

        private void CutsceneBuyHelper()
        {
            helperAppearVCam.gameObject.SetActive(true);
            view.gameObject.SetActive(true);
            view.sellStorageModule.productsStorage.SetActive(true);
            view.transform.localScale = Vector3.one * 0.01f;
            view.transform.DOScale(Vector3.one, 0.45f).SetEase(Ease.OutBack).SetLink(gameObject);
        }

        protected override void SetUpTasks()
        {
            base.SetUpTasks();
            
            _tasksGroup = new HelperTasksQueueGroup();
            _tasksGroup.Initialize(view, restTask);

            SetUpSellAnimalsProductsTask();
            SetUpInteractAnimalsTask();
            SetUpFishSellingTask();
        }

        private void SetUpSellAnimalsProductsTask()
        {
            _sellAnimalsProductsTask = new SellAnimalsProductsTask();
            _sellAnimalsProductsTask.Initialize(view, carrier, sellTask, containerAnimalProducts);
            
            containerAnimalProducts.onAddedProduct.On(() =>
            {
                _tasksGroup.RunTask(_sellAnimalsProductsTask);
            });
            
            if (containerAnimalProducts.products.Count > 0)
            {
                _tasksGroup.RunTask(_sellAnimalsProductsTask);
            }
        }
        
        private void SetUpInteractAnimalsTask()
        {
            _interactAnimalsTask = new InteractAnimalsTask();
            _interactAnimalsTask.Initialize(view, _animals);

            foreach (var animal in _animals)
            {
                animal.productionModule.onBecomeAvailable.On(() =>
                {
                    _tasksGroup.RunTask(_interactAnimalsTask);
                });
            }
            
            foreach (var animal in _animals)
            {
                if (animal.productionModule.interactItem.enabled)
                {
                    _tasksGroup.RunTask(_interactAnimalsTask);
                    break;
                }
            }
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
    }
}