using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Controllers
{
    public class TargetCameraOnObjectController : ControllerInternal
    {
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        protected MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        private CinemachineVirtualCamera _tileInBuyModeVCamCached;
        public CinemachineVirtualCamera tileInBuyModeVCam
        {
            get
            {
                if (_tileInBuyModeVCamCached == null)
                {
                    _tileInBuyModeVCamCached =  virtualCameraLinks.First(v => v.id == GameplaySettings.def.tileInBuyModeVCamIdConfig.id).virtualCamera;
                }
                return _tileInBuyModeVCamCached;
            }
        }
        
        public readonly TheSignal onShowCompleted = new();

        public void ShowObject(Transform target)
        {
            if(!tileInBuyModeVCam) return;
            tileInBuyModeVCam.m_Follow = target.transform;
            tileInBuyModeVCam.m_LookAt = target.transform;
            tileInBuyModeVCam.gameObject.SetActive(true);
            mainCharacterView.gameObject.SetActive(false);
            DOVirtual.DelayedCall(3f, () =>
            {
                tileInBuyModeVCam.gameObject.SetActive(false);
                mainCharacterView.gameObject.SetActive(true);
                DOVirtual.DelayedCall(2f, () =>
                {
                    onShowCompleted.Dispatch();
                }, false).SetLink(target.gameObject);
            }, false).SetLink(target.gameObject);
        }
    }
}