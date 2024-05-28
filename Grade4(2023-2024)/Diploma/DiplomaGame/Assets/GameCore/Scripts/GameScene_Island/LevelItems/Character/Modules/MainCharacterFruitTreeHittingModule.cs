using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.GameScene_Island.UI;
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

namespace GameCore.GameScene_Island.LevelItems.Character.Modules
{
    public class MainCharacterFruitTreeHittingModule : FruitTreeHittingModule
    {
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public TutorialArrow3D tutorialArrow3D { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        private CinemachineVirtualCamera _hittingAppleVCamCached;
        public CinemachineVirtualCamera hittingAppleVCam
        {
            get
            {
                if (_hittingAppleVCamCached == null)
                {
                    _hittingAppleVCamCached =  virtualCameraLinks.First(v => v.id == GameplaySettings.def.appleHittingVCamIdConfig.id).virtualCamera;
                }
                return _hittingAppleVCamCached;
            }
        }

        private float _lastDirEnd;

        private AppleHittingDialog _dialog;
        
        protected override float speedHitting => 12.5f;

        protected override void OnHittingStart(FruitTreeItem fruitTreeItem)
        {
            hittingAppleVCam.gameObject.SetActive(true);
            hittingAppleVCam.m_Follow = fruitTreeItem.camPoint;
            hittingAppleVCam.m_LookAt = fruitTreeItem.camPoint;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                windowFactory.Create<AppleHittingDialog>("AppleHitting", window =>
                {
                    _dialog = window;
                    window.Show();

                    window.onShowComplete.Once((_) =>
                    {
                        hittingCoroutine = StartCoroutine(HittingCoroutine());
                    });
                });
            },false).SetLink(gameObject);
            
            gameplayUIMenuWindow.Hide();
            tutorialArrow3D.Disable();
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);
        }

        protected override IEnumerator HittingCoroutine()
        {
            float moveCoeff = 0;
            float direction = 1;
            
            while (true)
            {
                yield return null;

                if (Input.touches.Length > 0)
                {
                    if (Input.touches[0].phase == TouchPhase.Moved)
                    {
                        Vector2 dir = Input.touches[0].deltaPosition.normalized;
                        if (dir.y > 0)
                        {
                            direction = 1;
                        }else
                        {
                            direction = -1;
                        }
                        
                        moveCoeff += Time.deltaTime * direction * speedHitting;
                        if (moveCoeff >= 1 || moveCoeff <= -1)
                        {
                            moveCoeff = direction;
                        }
                        
                        if(moveCoeff == _lastDirEnd) continue;
                        
                        if (moveCoeff >= 1 || moveCoeff <= -1 || moveCoeff == 0)
                        {
                            _lastDirEnd = moveCoeff;
                        }

                        onMove.Dispatch(moveCoeff);
                    }
                }
            }
        }
        
        protected override void EndHitting()
        {
            base.EndHitting();
            
            _dialog.Hide();
            hittingAppleVCam.gameObject.SetActive(false);
            
            gameplayUIMenuWindow.Show();
            tutorialArrow3D.Enable();
            popUpsController.containerUnderMenu.gameObject.SetActive(true);
            popUpsController.containerOverWindow.gameObject.SetActive(true);
        }
    }
}