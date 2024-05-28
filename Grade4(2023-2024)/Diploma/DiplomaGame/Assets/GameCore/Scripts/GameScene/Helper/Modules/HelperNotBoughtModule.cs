using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Blockers;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using UnityEngine;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSignals;
using Zenject;

namespace GameCore.GameScene.Helper.Modules
{
    public class HelperNotBoughtModule : CharacterModule
    {
        [SerializeField] private Transform _positionPoint;
        [SerializeField] private AnimatorParameterApplier _helpAnim;
        [SerializeField] private AnimatorParameterApplier _endHelpAnim;
        [SerializeField] private AnimatorParameterApplier _danceAnim;
        [SerializeField] private AnimatorParameterApplier _endDanceAnim;
        [SerializeField] private Transform _popUpPoint;
        [SerializeField] private GameObject _lifeBuoy;
        [SerializeField] private ParticleSystem _vfxIdleSwim;
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }
        [Inject, UsedImplicitly] public MenuBlockOverlay menuBlockOverlay { get; }
        
        private CinemachineVirtualCamera _helperVCamCached;
        public CinemachineVirtualCamera helperVCam
        {
            get
            {
                if (_helperVCamCached == null)
                {
                    _helperVCamCached =  virtualCameraLinks.First(v => v.id == GameplaySettings.def.helperVCamIdConfig.id).virtualCamera;
                }
                return _helperVCamCached;
            }
        }
        
        private CinemachineVirtualCamera _helperAppearVCamCached;
        public CinemachineVirtualCamera helperAppearVCam
        {
            get
            {
                if (_helperAppearVCamCached == null)
                {
                    _helperAppearVCamCached =  virtualCameraLinks.First(v => v.id == appearVCamConfig).virtualCamera;
                }
                return _helperAppearVCamCached;
            }
        }
        
        public virtual string appearVCamConfig => GameplaySettings.def.helperAppearVCamIdConfig.id;
        
        public HelperView view => character as HelperView;
        
        private TextPopUpView _helpPopUp;

        public readonly TheSignal onBuyCutsceneEnded = new();
        
        public void ActivateInternal()
        {
            character.transform.position = _positionPoint.position;
            character.transform.rotation = _positionPoint.rotation;
            
            character.GetModule<SwimModule>().SetSwim();
            _vfxIdleSwim.gameObject.SetActive(true);
            _vfxIdleSwim.Play();
            
            DOVirtual.DelayedCall(0.1f, () =>
            {
                _helpPopUp = popUpsController.SpawnUnderMenu<TextPopUpView>("HelpLifeBuoyPopUp");
                _helpPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
                _helpPopUp.worldSpaceConverter.followWorldObject = _popUpPoint;
                _helpPopUp.transform.localScale = Vector3.one * 0.01f;
                _helpPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            },false).SetLink(gameObject);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                _helpAnim.Apply();
            },false).SetLink(gameObject);
        }

        public void OnBuy()
        {
            menuBlockOverlay.Activate(this);
            helperAppearVCam.gameObject.SetActive(true);
            _helpPopUp.gameObject.SetActive(false);
            _endHelpAnim.Apply();

            DOVirtual.DelayedCall(2f, () =>
            {
                _lifeBuoy.gameObject.SetActive(true);
                Vector3 scale = _lifeBuoy.transform.localScale;
                _lifeBuoy.transform.localScale = Vector3.zero;
                _lifeBuoy.transform.DOScale(scale, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
                character.transform.DOMove(character.transform.forward * 2.5f, 2.5f).SetDelay(0.5f).SetRelative(true).SetLink(gameObject);
                
                DOVirtual.DelayedCall(2.5f, () =>
                {
                    helperAppearVCam.gameObject.SetActive(false);
                    helperVCam.gameObject.SetActive(true);
                    
                    _vfxIdleSwim.gameObject.SetActive(false);
                    
                    character.transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).SetLink(gameObject);

                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        _lifeBuoy.gameObject.SetActive(false);
                        
                        character.transform.position = view.logicModule.defaultPoint.position;
                        character.transform.rotation = view.logicModule.defaultPoint.rotation;
                
                        character.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
                    
                        character.GetModule<SwimModule>().SetNotSwim();
            
                        _danceAnim.Apply();

                        DOVirtual.DelayedCall(2f, () =>
                        {
                            _endDanceAnim.Apply();
                            onBuyCutsceneEnded.Dispatch();
                            DOVirtual.DelayedCall(0.5f, () =>
                            {
                                helperVCam.gameObject.SetActive(false);
                                menuBlockOverlay.Deactivate(this);
                            },false).SetLink(gameObject);
                        },false).SetLink(gameObject);
                    }, false).SetLink(gameObject);
                }, false).SetLink(gameObject);
            }, false).SetLink(gameObject);
           
        }
    }
}