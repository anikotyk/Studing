using System.Collections;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.Audio;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsCore.Game.Views.UI.Blockers;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class CutsceneArriveRaftManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public TutorialUIMenuWindow tutorialUIMenu { get; }
        [Inject, UsedImplicitly] public MenuBlockOverlay menuBlockOverlay { get; }
        [Inject, UsedImplicitly] public TopTextHint topTextHint { get; }
        [Inject, UsedImplicitly] public SpawnWoodInSeaManager spawnWoodInSeaManager { get; }
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public TutorialArrow3D tutorialArrow3D { get; }
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [Inject, UsedImplicitly] public BuyItemSound buyItemSound { get; }
        
        [SerializeField] private CinemachineVirtualCamera _startCam;
        [SerializeField] private CinemachineVirtualCamera _startCam2;
        [SerializeField] private CinemachineVirtualCamera _islandCam;
        [SerializeField] private CinemachineVirtualCamera _sharksCam;
        [SerializeField] private CinemachineVirtualCamera _sharksCam2;
        [SerializeField] private CinemachineVirtualCamera _characterCam;
        [SerializeField] private Transform _lockedIsland;
        
        [SerializeField] private Transform _girlChoose;
        [SerializeField] private Transform _boyChoose;
        
        [SerializeField] private GameObject _vfxChooseCharacter;
        
        [SerializeField] private GameObject[] _objectsToHideAfterShowScene;

        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        private TheSaveProperty<int> _activeBuyObjectIndexSaveProperty;
        private TheSaveProperty<bool> _watchedCutsceneSaveProperty;

        private void Awake()
        {
            _watchedCutsceneSaveProperty = new(CommStr.WatchedCutsceneArrive_Raft, linkToDispose: gameObject);
            _activeBuyObjectIndexSaveProperty = new(CommStr.ActiveBuyObjectIndex_Raft);
            
            if (!_watchedCutsceneSaveProperty.value && _activeBuyObjectIndexSaveProperty.value <= 0)
            {
                SwitchToCamera(_startCam);
            }
        }

        public override void Construct()
        {
            base.Construct();

            if (!_watchedCutsceneSaveProperty.value && _activeBuyObjectIndexSaveProperty.value <= 0)
            {
                initializeInOrderController.Add(ShowCutscene, 10000);
            }
            else
            {
                Deactivate();
            }
        }

        public void ShowCutscene()
        {
            StartCoroutine(CutsceneCoroutine());
        }

        private IEnumerator CutsceneCoroutine()
        {
            mainCharacterView.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            mainCharacterView.gameObject.SetActive(false);
            
            gameplayUIMenuWindow.gameObject.SetActive(false);
            gameplayUIMenuWindow.Hide();
            tutorialArrow3D.Disable();
            menuBlockOverlay.Activate(this);
            
            _lockedIsland.gameObject.SetActive(false);

            SwitchToCamera(_startCam);
            yield return new WaitForSeconds(0.1f);
            SwitchToCamera(_startCam2);
            yield return new WaitForSeconds(3.4f);
            SwitchToCamera(_islandCam);
            topTextHint.ShowHint("Dream Island");
            yield return new WaitForSeconds(2f);
            SwitchToCamera(_sharksCam);
            yield return new WaitForSeconds(2f);
            SwitchToCamera(_sharksCam2);
            yield return new WaitForSeconds(1.75f);
            SwitchToCamera(_characterCam);
            topTextHint.ShowHint("Deep Ocean");
            yield return new WaitForSeconds(2f);
            
            windowFactory.Create<ChooseCharacterTypeDialog>("ChooseCharacterType", window =>
            {
                window.Show();
                
                window.onHideStart.Once(_ =>
                {
                    _vfxChooseCharacter.SetActive(true);
                    buyItemSound.sound.Play();
                    mainCharacterView.gameObject.SetActive(true);
                    mainCharacterView.transform.localScale = Vector3.one * 0.01f;
                    mainCharacterView.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
                    _girlChoose.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetLink(gameObject);
                    _boyChoose.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetLink(gameObject);
                    
                    DeactivateAllCameras();
                    _lockedIsland.gameObject.SetActive(true);

                    DOVirtual.DelayedCall(2f, () =>
                    {
                        buyObjectsManager.SetInBuyModeCurrentObject();
                        spawnWoodInSeaManager.ActivateTutorialWoods();

                        menuBlockOverlay.Deactivate(this);
                        gameplayUIMenuWindow.Show();
                        tutorialArrow3D.Enable();
                        tutorialUIMenu.Show();
                        interactorCharactersCollection.mainCharacterView.GetModule<CharacterMovingModule>().onStartMoving.Once(() =>
                        {
                            tutorialUIMenu.Hide();
                            Deactivate();
                        });

                        _watchedCutsceneSaveProperty.value = true;
                    }, false).SetLink(gameObject);
                });
            }, false);
        }

        private void SwitchToCamera(CinemachineVirtualCamera cam)
        {
            DeactivateAllCameras();
            cam.gameObject.SetActive(true);
        }

        private void DeactivateAllCameras()
        {
            _startCam.gameObject.SetActive(false);
            _startCam2.gameObject.SetActive(false);
            _islandCam.gameObject.SetActive(false);
            _sharksCam.gameObject.SetActive(false);
            _sharksCam2.gameObject.SetActive(false);
            _characterCam.gameObject.SetActive(false);
        }

        private void Deactivate()
        {
            foreach (var obj in _objectsToHideAfterShowScene)
            {
                obj.SetActive(false);
            }
        }
    }
}