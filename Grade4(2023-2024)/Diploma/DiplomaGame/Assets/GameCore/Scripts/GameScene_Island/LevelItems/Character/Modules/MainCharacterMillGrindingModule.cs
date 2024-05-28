using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene_Island.UI;
using GameCore.GameScene.Settings;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Character.Modules
{
    public class MainCharacterMillGrindingModule : MillGrindingModule
    {
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public TutorialArrow3D tutorialArrow3D { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        protected override float speedGriding => 0.2f;
        
        private CinemachineVirtualCamera _grindingMillVCamCached;
        public CinemachineVirtualCamera grindingMillVCam
        {
            get
            {
                if (_grindingMillVCamCached == null)
                {
                    _grindingMillVCamCached =  virtualCameraLinks.First(v => v.id == GameplaySettings.def.millGrindingVCamIdConfig.id).virtualCamera;
                }
                return _grindingMillVCamCached;
            }
        }
        
        
        private MillGrindingDialog _dialog;

        protected override void OnStartGrinding(MillItem millItem)
        {
            grindingMillVCam.gameObject.SetActive(true);
            grindingMillVCam.m_Follow = millItem.camPoint;
            grindingMillVCam.m_LookAt = millItem.camPoint;
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                windowFactory.Create<MillGrindingDialog>("MillGrinding", window =>
                {
                    _dialog = window;
                    window.Show();

                    window.onShowComplete.Once((_) =>
                    {
                        grindingCoroutine = StartCoroutine(GrindingCoroutine());
                    });
                });
            },false).SetLink(gameObject);
            
            gameplayUIMenuWindow.Hide();
            tutorialArrow3D.Disable();
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);
        }
        
        protected override IEnumerator GrindingCoroutine()
        {
            float moveCoeff = 0;
            
            while (true)
            {
                yield return null;

                if (Input.touches.Length > 0)
                {
                    walkAnim.Apply();
                    moveCoeff += Time.deltaTime * speedGriding;
                    onMove.Dispatch(moveCoeff);
                }
                else
                {
                    stopWalkAnim.Apply();
                }
            }
        }
         
        protected override void OnEndGrinding()
        {
            base.OnEndGrinding();
            
            _dialog.Hide();
            grindingMillVCam.gameObject.SetActive(false);
            
            gameplayUIMenuWindow.Show();
            tutorialArrow3D.Enable();
            popUpsController.containerUnderMenu.gameObject.SetActive(true);
            popUpsController.containerOverWindow.gameObject.SetActive(true);
        }
    }
}