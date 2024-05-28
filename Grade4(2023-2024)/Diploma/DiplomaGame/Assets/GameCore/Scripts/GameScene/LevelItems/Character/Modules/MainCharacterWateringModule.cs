using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Settings;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Character.Modules
{
    public class MainCharacterWateringModule : WateringModule
    {
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public TutorialArrow3D tutorialArrow3D { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }

        private CinemachineVirtualCamera _wateringVCamCached;
        public CinemachineVirtualCamera wateringVCam
        {
            get
            {
                if (_wateringVCamCached == null)
                {
                    _wateringVCamCached =  virtualCameraLinks.First(v => v.id == GameplaySettings.def.wateringVCamIdConfig.id).virtualCamera;
                }
                return _wateringVCamCached;
            }
        }

        private WateringRunDialog _dialog;

        private Coroutine _runByClicksCoroutine;
        
        public override void StartWatering(WaterFilterObject waterFilterObject)
        {
            base.StartWatering(waterFilterObject);

            wateringVCam.m_Follow = waterFilterObject.camPoint;
            wateringVCam.m_LookAt = waterFilterObject.camPoint;
            wateringVCam.gameObject.SetActive(true);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                windowFactory.Create<WateringRunDialog>("WateringRunDialog", window =>
                {
                    _dialog = window;
                    window.Show();

                    window.onShowComplete.Once((_) =>
                    {
                        _runByClicksCoroutine = StartCoroutine(RunByClicksCoroutine());
                    });

                    window.onCancel.Once(CancelWatering);
                });
            },false).SetLink(gameObject);
            
            gameplayUIMenuWindow.Hide();
            tutorialArrow3D.Disable();
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);
        }
        
        protected override void OnStartWatering()
        {
            
        }
        
        public override void EndWatering()
        {
            base.EndWatering();
            
            _dialog.Hide();
            wateringVCam.gameObject.SetActive(false);
            
            gameplayUIMenuWindow.Show();
            tutorialArrow3D.Enable();
            popUpsController.containerUnderMenu.gameObject.SetActive(true);
            popUpsController.containerOverWindow.gameObject.SetActive(true);
            
            if (_runByClicksCoroutine != null)
            {
                StopCoroutine(_runByClicksCoroutine);
            }
        }

        public override void CancelWatering()
        {
            base.CancelWatering();
            
            _dialog.Hide();
            wateringVCam.gameObject.SetActive(false);
            
            gameplayUIMenuWindow.Show();
            tutorialArrow3D.Enable();
            popUpsController.containerUnderMenu.gameObject.SetActive(true);
            popUpsController.containerOverWindow.gameObject.SetActive(true);
            
            if (_runByClicksCoroutine != null)
            {
                StopCoroutine(_runByClicksCoroutine);
            }
        }

        private IEnumerator RunByClicksCoroutine()
        {
            float timeNeedHoldTouch = GameplaySettings.def.wateringData.timeNeedHoldTouch;
            
            float holdTime = 0f;
            bool isCharacterRunning = false;
            while (holdTime < timeNeedHoldTouch)
            {
                if (Input.touches.Length > 0)
                {
                    holdTime += Time.deltaTime;
                    _dialog.SetProgress(holdTime / timeNeedHoldTouch);
                    if (!isCharacterRunning)
                    {
                        isCharacterRunning = true;
                        _runWatering.Apply();
                        onStartRun.Dispatch();
                    }
                }
                else
                {
                    if (isCharacterRunning)
                    {
                        isCharacterRunning = false;
                        _stopRunWatering.Apply();
                        onStopRun.Dispatch();
                    }
                }
                yield return null;
            }

            EndWatering();
        }
    }
}