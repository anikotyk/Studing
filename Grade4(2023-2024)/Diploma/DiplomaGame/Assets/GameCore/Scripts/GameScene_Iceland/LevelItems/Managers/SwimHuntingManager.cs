using Cinemachine;
using DG.Tweening;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.Common.UI;
using GameCore.GameScene_Iceland.LevelItems.Items;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Sensors;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class SwimHuntingManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public TargetCameraOnObjectController targetCameraOnObjectController { get; }
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public InuitsGoSwimHuntingCutscene inuitsGoSwimHuntingCutscene { get; }
        [Inject, UsedImplicitly] public WhaleKillerCutscene whaleKillerCutscene { get; }
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public TutorialArrow3D tutorialArrow3D { get; }
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        
        private InteractorCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;

        [SerializeField] private BoatHuntingView _boat;
        [SerializeField] private Transform _boatDefaultPoint;
        [SerializeField] private Transform _characterAfterHuntingPoint;
        [SerializeField] private GameObject _hunters;
        [SerializeField] private BuyObject _objectActivateHuting;
        [SerializeField] private GameObject[] _objectsActivateHuting;
        [SerializeField] private StartHuntingDonatePlatform _startHuntingDonatePlatform;
        
        [SerializeField] private CinemachineVirtualCamera _boatCam;
        [SerializeField] private CinemachineVirtualCamera _boatCloseCam;
        
        [SerializeField] private AudioSource[] _musicTurnOff;
        [SerializeField] private AudioSource[] _musicTurnOn;

        private bool _isHuntingNow;
        public bool isHuntingNow => _isHuntingNow;

        private CancelDialog _cancelDialog;

        public override void Construct()
        {
            base.Construct();
            _boat.walkerController.TurnOffMovement();
            _hunters.gameObject.SetActive(false);
            initializeInOrderController.Add(Initialize, 2000);
        }

        private void Initialize()
        {
            if (!_objectActivateHuting.isBought)
            {
                _objectActivateHuting.onBuy.Once(()=>
                {
                    ActivateHunting();
                    if (_objectActivateHuting.GetComponent<IWindowShowable>() != null)
                    {
                        _objectActivateHuting.GetComponent<IWindowShowable>().onWindowClosed.Once(() =>
                        {
                            targetCameraOnObjectController.ShowObject(_startHuntingDonatePlatform.transform);
                            targetCameraOnObjectController.onShowCompleted.Once(() =>
                            {
                                buyObjectsManager.ShowCurrentBuyObject();
                            });
                        });
                    }
                    else
                    {
                        targetCameraOnObjectController.ShowObject(_startHuntingDonatePlatform.transform);
                    }
                });
                DeactivateHunting();
            }
            else
            {
                ActivateHunting();
            }
        }

        public void StartHunting()
        {
            hub.Get<GCSgnl.SwimHuntingSignals.Started>().Dispatch();
            
            gameplayUIMenuWindow.Hide();
            tutorialArrow3D.Disable();
            mainCharacterView.gameObject.SetActive(false);
            mainCharacterView.GetComponent<MainCharacterSimpleWalkerController>().TurnOffMovement();
            mainCharacterView.GetModule<SurroundInteractorCharacterSensorModule>().sensor.enabled = false;

            DOVirtual.DelayedCall(0.5f, () =>
            {
                inuitsGoSwimHuntingCutscene.StartCutscene();
                inuitsGoSwimHuntingCutscene.onEndedScene.Once(() =>
                {
                    foreach (var music in _musicTurnOff)
                    {
                        music.Stop();
                    }
                    foreach (var music in _musicTurnOn)
                    {
                        music.Play();
                    }
                    _boatCam.gameObject.SetActive(true);
                    _boatCloseCam.gameObject.SetActive(true);
                    _hunters.gameObject.SetActive(true);
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        whaleKillerCutscene.StartCutscene();
                        _boatCloseCam.gameObject.SetActive(false);
                        whaleKillerCutscene.onEndedScene.Once(() =>
                        {
                            _isHuntingNow = true;
                            /*windowFactory.Create<CancelDialog>("CancelDialog", window =>
                            {
                                _cancelDialog = window;
                                window.Show();
                                window.onCancel.Once(CancelHunting);
                            });*/
                            _boat.walkerController.TurnOnMovement();
                        });
                    }, false).SetLink(gameObject);
                    
                });
            }, false).SetLink(gameObject);
        }

        private void CancelHunting()
        {
            StopHunting();
        }
        
        public void StopHunting()
        {
            if (_cancelDialog != null)
            {
                _cancelDialog.Hide();
            }

            _isHuntingNow = false;
            hub.Get<GCSgnl.SwimHuntingSignals.Ended>().Dispatch();
            mainCharacterView.transform.position = _characterAfterHuntingPoint.position;
            mainCharacterView.gameObject.SetActive(true);
            mainCharacterView.GetComponent<MainCharacterSimpleWalkerController>().TurnOnMovement();
            mainCharacterView.GetModule<SurroundInteractorCharacterSensorModule>().sensor.enabled = true;
            _boatCam.gameObject.SetActive(false);
            _boat.walkerController.TurnOffMovement();
            gameplayUIMenuWindow.Show();
            tutorialArrow3D.Enable();
            _boat.ResetView();
            _boat.transform.position = _boatDefaultPoint.position;
            _boat.transform.rotation = _boatDefaultPoint.rotation;
            _boat.visual.transform.localRotation = Quaternion.Euler(0, 90, 0);
            _boat.transform.localScale = Vector3.one;
            _boat.gameObject.SetActive(true);
            _hunters.gameObject.SetActive(false);
            
            foreach (var music in _musicTurnOff)
            {
                music.Play();
            }
            foreach (var music in _musicTurnOn)
            {
                music.Stop();
            }
        }

        private void DeactivateHunting()
        {
            foreach (var obj in _objectsActivateHuting)
            {
                obj.SetActive(false);
            }
        }
        
        private void ActivateHunting()
        {
            foreach (var obj in _objectsActivateHuting)
            {
                obj.SetActive(true);
            }
            
            _startHuntingDonatePlatform.ActivateInternal(true);
        }

        private void ShowHuntingSpot()
        {
            
        }
    }
}