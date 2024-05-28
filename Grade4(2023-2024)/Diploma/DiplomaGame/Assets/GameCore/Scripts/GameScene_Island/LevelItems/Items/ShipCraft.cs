using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.GameScene_Island.Saves;
using GameCore.GameScene.Settings;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using ModestTree;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Blockers;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class ShipCraft : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public MenuBlockOverlay menuBlockOverlay { get; }
        [Inject, UsedImplicitly] public ShipStagesSaveData shipStagesSaveData { get; }
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public TutorialArrow3D tutorialArrow3D { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public TopTextHint topTextHint { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        [SerializeField] private BuyObject _activateRuinedShipObject;
        [SerializeField] private GameObject _shipStageRuined;
        [SerializeField] private GameObject _ship;
        [SerializeField] private Transform _camPoint;
        [SerializeField] private GameObject _vfxFixRuined;
        [SerializeField] private ParticleSystem _vfxNewStage;
        [SerializeField] private GameObject _vfxShipReady;
        [SerializeField] private AudioSource _soundNewStage;
        [SerializeField] private AudioSource _soundSpawn;
        [SerializeField] private AudioSource _soundReadyShip;
        
        public GameObject shipStageRuined => _shipStageRuined;
        [SerializeField] private Transform[] _shipStages;
        public int activeStagesCount => _shipStages.Count(stage => stage.gameObject.activeSelf);
        [SerializeField] private GameObject _dock;
        
        [SerializeField] private AudioSource[] _audiosToStop;

        private CinemachineVirtualCamera _shipVCamCached;
        public CinemachineVirtualCamera shipVCam
        {
            get
            {
                if (_shipVCamCached == null)
                {
                    _shipVCamCached =  virtualCameraLinks.First(v => v.id == GameplaySettings.def.shipVCamIdConfig.id).virtualCamera;
                }
                return _shipVCamCached;
            }
        }
        
        public TheSignal onFullShipComplete { get; } = new();

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Intialize, 2000);
        }

        private void Intialize()
        {
            ValidateStages();
        }

        private void ValidateStages()
        {
            if (shipStagesSaveData.value.IsNoStagesActive())
            {
                if (_activateRuinedShipObject.isBought)
                {
                    _shipStageRuined.gameObject.SetActive(true);
                }
                else
                {
                    _shipStageRuined.gameObject.SetActive(false);
                }
                _dock.gameObject.SetActive(false);
              
                foreach (var stage in _shipStages)
                {
                    stage.gameObject.SetActive(false);
                }
            }
            else
            {
                _dock.gameObject.SetActive(true);
                _shipStageRuined.gameObject.SetActive(false);

                for (int i = 0; i < _shipStages.Length; i++)
                {
                    if (shipStagesSaveData.value.IsActive(i))
                    {
                        _shipStages[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        _shipStages[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        public bool IsStageActive(Transform stage)
        {
            return shipStagesSaveData.value.IsActive(_shipStages.IndexOf(stage));
        }

        public void ActivateShipStage(Transform stage)
        {
            _dock.gameObject.SetActive(true);
            shipStagesSaveData.value.SetActive(_shipStages.IndexOf(stage));

            foreach (var audio in _audiosToStop)
            {
                audio.Stop();
            }

            stage.gameObject.SetActive(true);
            stage.localScale = Vector3.zero;

            mainCharacterView.GetComponent<MainCharacterSimpleWalkerController>().TurnOffMovement();
            gameplayUIMenuWindow.Hide();
            tutorialArrow3D.Disable();
            menuBlockOverlay.Activate(this);
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                shipVCam.gameObject.SetActive(true);
                if (_vfxNewStage != null)
                {
                    Vector3 pos = stage.transform.position;
                    pos.y = _vfxNewStage.transform.position.y;
                    _vfxNewStage.transform.position = pos;
                    shipVCam.m_Follow = _vfxNewStage.transform;
                    shipVCam.m_LookAt = _vfxNewStage.transform;
                }
                DOVirtual.DelayedCall(2.75f, () =>
                {
                    float delay = 0;
                    if (_shipStageRuined.gameObject.activeSelf)
                    {
                        _vfxFixRuined.SetActive(true);
                        _shipStageRuined.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
                        {
                            _shipStageRuined.gameObject.SetActive(false);
                        }).SetEase(Ease.InBack).SetLink(gameObject);
                    }
                    else
                    {
                        if (_vfxNewStage != null)
                        {
                            _vfxNewStage.Play();
                            delay = 0.15f;
                            if (_soundNewStage != null)
                            {
                                _soundNewStage.Play();
                            }
                        }
                    }

                    DOVirtual.DelayedCall(delay, () =>
                    {
                        stage.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                        {
                            topTextHint.ShowHint("Ship part " + activeStagesCount + "/" + _shipStages.Length, showTime : 3f , fadeValue : 0.9f);
                            if (IsFullShipComplete())
                            {
                               OnFullShipReady();
                            }
                        }).SetLink(gameObject);
                        if (_soundSpawn != null)
                        {
                            _soundSpawn.Play();
                        }
                    }, false).SetLink(gameObject);
                    
                    if (IsFullShipComplete())
                    {
                        delay += 2f;
                    }

                    DOVirtual.DelayedCall(2f + delay, () =>
                    {
                        shipVCam.gameObject.SetActive(false);
                        DOVirtual.DelayedCall(1f, () =>
                        {
                            gameplayUIMenuWindow.Show();
                            tutorialArrow3D.Enable();
                            menuBlockOverlay.Deactivate(this);
                            popUpsController.containerUnderMenu.gameObject.SetActive(true);
                            popUpsController.containerOverWindow.gameObject.SetActive(true);
                            mainCharacterView.GetComponent<MainCharacterSimpleWalkerController>().TurnOnMovement();
                        }, false).SetLink(gameObject);
                        
                        foreach (var audio in _audiosToStop)
                        {
                            audio.Play();
                        }
                    }, false).SetLink(gameObject);
                }, false).SetLink(gameObject);
            }, false).SetLink(gameObject);
        }

        private void OnFullShipReady()
        {
            onFullShipComplete.Dispatch();
            
            shipVCam.m_Follow = _camPoint;
            shipVCam.m_LookAt = _camPoint;
            
            CinemachineComponentBase componentBase = shipVCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is CinemachineFramingTransposer)
            {
                (componentBase as CinemachineFramingTransposer).m_CameraDistance = 9;
            }
            
            DOVirtual.DelayedCall(0.25f, () =>
            {
                _ship.transform.DOScale(Vector3.one * 1.1f, 2.2f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    _ship.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InBack).SetLink(gameObject);
                    if (componentBase is CinemachineFramingTransposer)
                    {
                        (componentBase as CinemachineFramingTransposer).m_CameraDistance = 7;
                    }
                }).SetLink(gameObject);
                _vfxShipReady.SetActive(true);
                _soundReadyShip.Play();
            }, false).SetLink(gameObject);
        }

        public bool IsFullShipComplete()
        {
            return activeStagesCount >= _shipStages.Length;
        }

        public void CompleteShipCheat()
        {
            for (int i = 0; i < _shipStages.Length; i++)
            {
                shipStagesSaveData.value.SetActive( i);
            }
        }
    }
}