using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.GameScene_Island.UI;
using GameCore.GameScene.Settings;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Blockers;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class Recipe : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public MenuBlockOverlay menuBlockOverlay { get; }
        [Inject, UsedImplicitly] public TutorialArrow3D tutorialArrow3D { get; }
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }

        [SerializeField] private BuyObject _buyObjectToActivate;
        [SerializeField] private BuyObject _buyObjectToBuy;
        [SerializeField] private string _dialogId;

        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }
        
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

        private bool _isUsed;
        
        private bool _isActive;
        public bool isActive => _isActive;
        
        public TheSignal onActivate { get; } = new();
        public TheSignal onTaken { get; } = new();

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 10000);
        }

        private void Initialize()
        {
            _isActive = true;
            
            if (_buyObjectToBuy.isBought)
            {
                _isUsed = true;
                DeactivateInternal();
            }
            else if (!_buyObjectToActivate.isBought)
            {
                DeactivateInternal();
                _buyObjectToActivate.onBuy.Once(() =>
                {
                    Activate();
                });
            }
        }
        
        public void OnTaken()
        {
            interactItem.enabled = false;

            if (_isUsed)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _isUsed = true;
           
            transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).SetLink(gameObject);

            gameplayUIMenuWindow.Hide();
            tutorialArrow3D.Disable();
            menuBlockOverlay.Activate(this);

            onTaken.Dispatch();

            DOVirtual.DelayedCall(1f, () =>
            {
                windowFactory.Create<RecipeDialog>(_dialogId, window =>
                {
                    window.onHideComplete.Once((_) =>
                    {
                        workbenchVCam.gameObject.SetActive(true);
                        DOVirtual.DelayedCall(2f, () =>
                        {
                            _buyObjectToBuy.Buy();
                            DOVirtual.DelayedCall(1f, () =>
                            {
                                workbenchVCam.gameObject.SetActive(false);
                                gameplayUIMenuWindow.Show();
                                tutorialArrow3D.Enable();
                                menuBlockOverlay.Deactivate(this);
                                gameObject.SetActive(false);
                            }).SetLink(gameObject);
                        }).SetLink(gameObject);
                    });
                
                    window.Show();
                });
            }, false).SetLink(gameObject);
        }

        private void DeactivateInternal()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }

        private void Activate()
        {
            _isActive = true;
            onActivate.Dispatch();
            gameObject.SetActive(true);
            transform.localScale = Vector3.one * 0.01f;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
        }
    }
}