using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class StallCutsceneManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public StallObject stallObject { get; }
        [Inject, UsedImplicitly] public SellersManager sellersManager { get; }
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        [SerializeField] private ProductDataConfig _spawnAtBuy;
        [SerializeField] private float _cutSceneTime = 6.5f;

        private CinemachineVirtualCamera _stallVCamCached;
        public CinemachineVirtualCamera stallVCam
        {
            get
            {
                if (_stallVCamCached == null)
                {
                    _stallVCamCached =  virtualCameraLinks.First(v => v.id == GameplaySettings.def.stallVCamIdConfig.id).virtualCamera;
                }
                return _stallVCamCached;
            }
        }
        
        public readonly TheSignal onCutsceneEnded = new();

        public override void Construct()
        {
            base.Construct();
            
            stallObject.buyObject.onBuy.Once(OnBuy);
        }
        
        private void OnBuy()
        {
            stallObject.sellPlatform.interactItem.enabled = false;
            
            stallVCam.gameObject.SetActive(true);
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);

            mainCharacterView.gameObject.SetActive(false);
            
            sellersManager.StartFirstSeller();

            for (int i = 0; i < 5; i++)
            {
                ProductView prod = spawnProductsManager.Spawn(_spawnAtBuy);
                prod.transform.SetParent(stallObject.carrier.container);
              
                if(prod is SellProduct)
                {
                    (prod as SellProduct).ResetView();
                }

                stallObject.carrier.Add(prod, false);
            }

            DOVirtual.DelayedCall(_cutSceneTime, () =>
            {
                stallVCam.gameObject.SetActive(false);
                popUpsController.containerUnderMenu.gameObject.SetActive(true);
                popUpsController.containerOverWindow.gameObject.SetActive(true);
                mainCharacterView.gameObject.SetActive(true);
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    stallObject.sellPlatform.interactItem.enabled = true;
                }, false).SetLink(gameObject);
                onCutsceneEnded.Dispatch();
            },false).SetLink(gameObject);
        }
    }
}