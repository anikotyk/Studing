using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.Helper;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Managers;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Sensors;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class SwimToIslandManager : Cutscene
    {
        [SerializeField] private VirtualCameraIdDataConfig _showWaterFilter2CamId;
        [SerializeField] private VirtualCameraIdDataConfig _startSwimCamId;
        [SerializeField] private VirtualCameraIdDataConfig _swimRaftCamId;
        [SerializeField] private GameObject _waterFilter2;
        [SerializeField] private GameObject _waterFilter1;
        [SerializeField] private Transform _pointMainCharacter;
        [SerializeField] private Transform _pointHelper;
        [SerializeField] private HelperView _helperView;
        [SerializeField] private Vector3 _rotateAngle;
        [SerializeField] private GameObject _vfx;
        [SerializeField] private Transform _moveDirection;
        [SerializeField] private Transform _lockedIsland;
        [SerializeField] private BuyObject _buyObject;

        [Inject, UsedImplicitly] public SceneLoader sceneLoader { get; }
        [Inject, UsedImplicitly] public WaterFilterObject waterFilterObject { get; }
        [Inject, UsedImplicitly] public SpawnWoodInSeaManager spawnWoodInSeaManager { get; }
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public SharksManager sharksManager { get; }
        [InjectOptional, UsedImplicitly] public SellersManager sellersManager { get; }
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }

        private CinemachineVirtualCamera _waterFilter2VCamCached;
        public CinemachineVirtualCamera waterFilter2VCam
        {
            get
            {
                if (_waterFilter2VCamCached == null)
                {
                    _waterFilter2VCamCached =  virtualCameraLinks.First(v => v.id == _showWaterFilter2CamId.id).virtualCamera;
                }
                return _waterFilter2VCamCached;
            }
        }
        
        private CinemachineVirtualCamera _startSwimVCamCached;
        public CinemachineVirtualCamera startSwimVCam
        {
            get
            {
                if (_startSwimVCamCached == null)
                {
                    _startSwimVCamCached =  virtualCameraLinks.First(v => v.id == _startSwimCamId.id).virtualCamera;
                }
                return _startSwimVCamCached;
            }
        }
        
        private CinemachineVirtualCamera _swimRaftVCamCached;
        public CinemachineVirtualCamera swimRaftVCam
        {
            get
            {
                if (_swimRaftVCamCached == null)
                {
                    _swimRaftVCamCached =  virtualCameraLinks.First(v => v.id == _swimRaftCamId.id).virtualCamera;
                }
                return _swimRaftVCamCached;
            }
        }
        
        protected override bool deactivateMainCharacter => false;
        
        public override void Construct()
        {
            base.Construct();
            
            _buyObject.onBuy.Once(StartCutscene);
            
            initializeInOrderController.Add(CheckForCutscene, 10000);
        }

        private void CheckForCutscene()
        {
            if (_buyObject.isBought)
            {
                StartCutscene();
            }
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            sellersManager.TurnOffSellers();
            sharksManager.gameObject.SetActive(false);
            spawnWoodInSeaManager.gameObject.SetActive(false);
            spawnProductsManager.container.gameObject.SetActive(false);
            mainCharacterView.GetComponent<MainCharacterSimpleWalkerController>().TurnOffMovement();
            mainCharacterView.bodyCollider.enabled = false;
            mainCharacterView.GetModule<SurroundInteractorCharacterSensorModule>().sensor.enabled = false;
            mainCharacterView.GetModule<StackModule>().HideStack(true);
            if (mainCharacterView.GetModule<SwimModule>().isSwim)
            {
                mainCharacterView.GetModule<SwimModule>().SetNotSwim();
            }
            mainCharacterView.model.BlockInteractions(this);
            mainCharacterView.GetModule<CharacterMovingModule>().enabled = false;
            
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);

            _lockedIsland.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _lockedIsland.gameObject.SetActive(false);
            }).SetLink(gameObject);

            mainCharacterView.transform.DOLookAt(_pointMainCharacter.position, 0.5f).SetLink(gameObject);
            
            waterFilterObject.GetComponent<InteractItem>().enabled = false;
            
            _helperView.GetModule<WateringModule>().StopWateringInternal();
            _helperView.logicModule.TurnOffHelper();
            _helperView.bodyCollider.enabled = false;
            _helperView.GetModule<StackModule>().HideStack(true);
            _helperView.GetModule<SurroundInteractorCharacterSensorModule>().sensor.enabled = false;

            _helperView.transform.DOLookAt(_pointHelper.position, 0.5f).SetLink(gameObject);
            _helperView.model.BlockInteractions(this);
            
            waterFilter2VCam.gameObject.SetActive(true);
           
            yield return new WaitForSeconds(2.5f);
            waterFilter2VCam.gameObject.SetActive(false);
            startSwimVCam.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            mainCharacterView.GetModule<WateringModule>().runWatering.Apply();
            mainCharacterView.GetModule<StackModule>().HideStack(true);
            _helperView.GetModule<WateringModule>().runWatering.Apply();
            _helperView.GetModule<StackModule>().HideStack(true);

            float speedMove = 1.35f;
           
            float timeMove = Vector3.Distance(mainCharacterView.transform.position, _pointMainCharacter.position) / speedMove;
            float timeMoveWait = timeMove;
            mainCharacterView.transform.DOMove(_pointMainCharacter.position, timeMove).SetEase(Ease.Linear).OnComplete(() =>
            {
                mainCharacterView.GetModule<WateringModule>().stopRunWatering.Apply();
                mainCharacterView.transform.DORotate(_pointMainCharacter.rotation.eulerAngles, 0.25f).SetLink(gameObject);
            }).SetLink(gameObject);
            
            float timeMoveHelper = Vector3.Distance(_helperView.transform.position, _pointHelper.position) / speedMove;
            timeMoveWait = timeMoveHelper > timeMoveWait ? timeMoveHelper : timeMoveWait;
            _helperView.transform.DOMove(_pointHelper.position, timeMoveHelper).SetEase(Ease.Linear).OnComplete(() =>
            {
                _helperView.GetModule<WateringModule>().stopRunWatering.Apply();
                _helperView.transform.DORotate(_pointHelper.rotation.eulerAngles, 0.25f).SetLink(gameObject);
            }).SetLink(gameObject);

            yield return new WaitForSeconds(timeMoveWait + 0.25f);
            
            mainCharacterView.transform.SetParent(transform);
            mainCharacterView.GetModule<WateringModule>().runWatering.Apply();
            
            _helperView.transform.SetParent(transform);
            _helperView.GetModule<WateringModule>().runWatering.Apply();
            
            _waterFilter2.GetComponent<Animator>().enabled = true;
            _waterFilter1.GetComponent<Animator>().enabled = true;
            
            _vfx.SetActive(true);

            transform.DORotate(_rotateAngle, 0.5f).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
           
            startSwimVCam.gameObject.SetActive(false);
            swimRaftVCam.gameObject.SetActive(true);
            
            topTextHint.ShowHint("Swim to island");

            transform.DOMove(_moveDirection.forward, 0.5f).SetRelative(true).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetLink(gameObject);
            
            yield return new WaitForSeconds(2f);

            bool isBlackWindowShown = false;
            float delayBlackWindow = 2f;
           
            windowFactory.Create<ChapterCompleteDialog>("ChapterComplete", window =>
            {
                window.SetChapterNumber(1);
                window.Show();
            });
            
            DOVirtual.DelayedCall(delayBlackWindow, () =>
            {
                if (!isBlackWindowShown)
                {
                    windowFactory.Create<UIDialogWindow>("BlackScreen", window =>
                    {
                        isBlackWindowShown = true;
                        window.Show();
                        window.onShowComplete.Once((_) =>
                        {
                            sceneLoader.Load(CommStr.GameScene_Island);
                        });
                    });
                }
            },false).SetLink(gameObject);
        }
    }
}