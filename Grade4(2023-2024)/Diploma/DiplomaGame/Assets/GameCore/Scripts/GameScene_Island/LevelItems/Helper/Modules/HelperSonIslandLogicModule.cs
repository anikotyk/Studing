using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.Misc;
using GameCore.Common.UI;
using GameCore.GameScene_Island.LevelItems.Animal;
using GameCore.GameScene_Island.LevelItems.Helper.Tasks;
using GameCore.GameScene_Island.LevelItems.Managers;
using GameCore.GameScene_Island.LevelItems.Platforms;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Helper.Modules
{
    public class HelperSonIslandLogicModule : HelperLogicModule
    {
        [SerializeField] private BuyObject _objectTurnOnHelper;
        [SerializeField] private Transform _appearPoint;
        [SerializeField] private HelperSonCutsceneManager _helperSonCutsceneManager;
        [SerializeField] private AudioSource _appearLongSound;
        [SerializeField] private AudioSource _appearShortSound;
        [SerializeField] private AnimalProductingView[] _animals;
        [SerializeField] private Sprite _helperSonSprite;
        [SerializeField] private string _helperSonAppearText;
        [Inject, UsedImplicitly] public VfxStack vfxStack { get; }
        [Inject, UsedImplicitly] public ContainerAnimalProducts containerAnimalProducts { get; }
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }

        public override string appearVCamConfig => GameplaySettings.def.helperSonAppearVCamIdConfig.id;
        
        private CutItemsTask _cutItemsTask;
        private GetCutItemsTask _getCutItemsTask;
        
        private FeedAnimalsTask _feedAnimalsTask;
        private InteractAnimalsTask _interactAnimalsTask;
        private SellAnimalsProductsTask _sellAnimalsProductsTask;

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
            _helperSonCutsceneManager.StartCutscene();
            
            _helperSonCutsceneManager.onCutsceneEnded.Once(() =>
            {
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
                                window.Initialize(_helperSonSprite, _helperSonAppearText);
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
            });
        }

        protected override void SetUpTasks()
        {
            base.SetUpTasks();
            
            _tasksGroup = new HelperTasksQueueGroup();
            _tasksGroup.Initialize(view, restTask);

            SetUpCutItemsTask();
            SetUpGetCutItemsTask();
            
            SetUpFeedAnimalsTask();
            SetUpSellAnimalsProductsTask();
            SetUpInteractAnimalsTask();
        }

        private void SetUpCutItemsTask()
        {
            _cutItemsTask = new CutItemsTask();
            _cutItemsTask.Initialize(view);
        }
        
        private void SetUpGetCutItemsTask()
        {
            _getCutItemsTask = new GetCutItemsTask();
            _getCutItemsTask.Initialize(view, carrier);
        }
        
        private void SetUpFeedAnimalsTask()
        {
            _feedAnimalsTask = new FeedAnimalsTask();
            _feedAnimalsTask.Initialize(view, carrier, _cutItemsTask, _getCutItemsTask, sellTask);

            foreach (var feedPlatform in GameObject.FindObjectsOfType<FeedPlatform>(true))
            {
                feedPlatform.onHasSpace.On(() =>
                {
                    _tasksGroup.RunTask(_feedAnimalsTask);
                });
            }
            
            foreach (var feedPlatform in GameObject.FindObjectsOfType<FeedPlatform>(true))
            {
                if (feedPlatform.IsHasSpace())
                {
                    _tasksGroup.RunTask(_feedAnimalsTask);
                    break;
                }
            }
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
    }
}