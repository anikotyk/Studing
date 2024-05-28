using System.Collections;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.Common.UI;
using GameCore.GameScene.Helper;
using GameCore.GameScene.LevelItems;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Managers
{
    public class ArriveIslandCutsceneManager : Cutscene
    {
        [Inject, UsedImplicitly] public TutorialUIMenuWindow tutorialUIMenu { get; }
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] private HelperView helperView { get; }
        [Inject, UsedImplicitly] private StartWoodIsland startWoodIsland { get; }
        
        [SerializeField] private CinemachineVirtualCamera _startCam;
        [SerializeField] private CinemachineVirtualCamera _lighteningCam;
        [SerializeField] private CinemachineVirtualCamera _flyCam;
        [SerializeField] private CinemachineVirtualCamera _endCam;
        
        [SerializeField] private Transform _endCharacterPoint;
        [SerializeField] private AnimatorParameterApplier _layAnimApplier;
        [SerializeField] private AnimatorParameterApplier _layStopAnimApplier;
        [SerializeField] private AnimatorParameterApplier _flyAnimApplier;
        [SerializeField] private AnimatorParameterApplier _flyHelperAnimApplier;
        [SerializeField] private GameObject _firstTile;
        [SerializeField] private GameObject _raft;
        [SerializeField] private GameObject _helperCutsceneGO;
        [SerializeField] private GameObject _mainCharacterCutsceneGO;
        
        [SerializeField] private GameObject _waterDay;
        [SerializeField] private GameObject _waterNight;
        
        [SerializeField] private Material _skyboxNight;
        
        [SerializeField] private ParticleSystem _lighteningVfx;
        [SerializeField] private AudioSource _lighteningSound;
        
        [SerializeField] private AudioSource _rainSound;
        [SerializeField] private AudioSource _tornadoSound;
        [SerializeField] private AudioSource[] _gameplayAudios;
        
        [SerializeField] private GameObject[] _objectsToHideAfterShowScene;
        
        private TheSaveProperty<int> _activeBuyObjectIndexIslandSaveProperty;
        private TheSaveProperty<bool> _watchedCutsceneIslandArriveSaveProperty;

        private void Awake()
        {
            _watchedCutsceneIslandArriveSaveProperty = new(CommStr.WatchedCutsceneArrive_Island, linkToDispose: gameObject);
            _activeBuyObjectIndexIslandSaveProperty = new(CommStr.ActiveBuyObjectIndex_Island);
            
            if (!_watchedCutsceneIslandArriveSaveProperty.value && _activeBuyObjectIndexIslandSaveProperty.value <= 0)
            {
                SwitchToCamera(_startCam.gameObject);
            }
        }

        public override void Construct()
        {
            base.Construct();

            if (!_watchedCutsceneIslandArriveSaveProperty.value && _activeBuyObjectIndexIslandSaveProperty.value <= 0)
            {
                initializeInOrderController.Add(StartCutscene, 10000);
            }
            else
            {
                Deactivate();
            }
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            UIDialogWindow blackScreen = null;
            windowFactory.Create<UIDialogWindow>("BlackScreen", (window) =>
            {
                blackScreen = window;
            });
            
            gameplayUIMenuWindow.gameObject.SetActive(false);
            
            _raft.SetActive(true);
            _firstTile.SetActive(false);
            
            _rainSound.Play();
            _tornadoSound.Play();
            foreach (var audio in _gameplayAudios)
            {
                audio.Stop();
            }
            
            Material skyboxMat = RenderSettings.skybox;
            RenderSettings.skybox = _skyboxNight;
            _waterNight.SetActive(true);
            _waterDay.SetActive(false);
            
            _helperCutsceneGO.SetActive(true);
            mainCharacterView.gameObject.SetActive(false);
            helperView.gameObject.SetActive(false);
            
            SwitchToCamera(_startCam.gameObject);
            yield return new WaitForSeconds(0.1f);
            topTextHint.ShowHint("Horrible storm");
            yield return new WaitForSeconds(3f);
            
            SwitchToCamera(_lighteningCam.gameObject);
            yield return new WaitForSeconds(0.25f);
            _lighteningSound.Play();
            _lighteningVfx.Play();

            for (int i = 0; i < _raft.transform.childCount; i++)
            {
                Vector3 pos = _raft.transform.position + Vector3.right * Random.Range(-5f, 5f) +
                              Vector3.forward * Random.Range(-5f, 5f) + Vector3.up * -2f;
                Transform tile = _raft.transform.GetChild(i);
                tile.DOJump(pos, 6f, 1, 3.5f).OnComplete(() =>
                {
                    tile.DOScale(Vector3.zero, 0.5f).SetLink(gameObject);
                }).SetLink(gameObject);
            }

            _mainCharacterCutsceneGO.transform.DOMoveY(6f, 3f).OnComplete(() =>
            {
                _mainCharacterCutsceneGO.transform.DOMoveY(-3f, 3f).SetLink(gameObject);
            }).SetLink(gameObject);
            _mainCharacterCutsceneGO.transform.DORotate(new Vector3(0, 90, 0), 1f).SetLink(gameObject);
            
            _helperCutsceneGO.transform.DOMoveY(6.5f, 3f).OnComplete(() =>
            {
                _helperCutsceneGO.transform.DOMoveY(-6.5f, 4f).SetLink(gameObject);
            }).SetLink(gameObject);
            _helperCutsceneGO.transform.DORotate(new Vector3(0, -90, 0), 1f).SetLink(gameObject);
            
            _flyAnimApplier.Apply();
            _flyHelperAnimApplier.Apply();
            
            yield return new WaitForSeconds(1f);
            SwitchToCamera(_flyCam.gameObject);
            yield return new WaitForSeconds(1.5f);
            Time.timeScale = 0.25f;
            yield return new WaitForSeconds(0.4f);
            Time.timeScale = 1f;
            _mainCharacterCutsceneGO.transform.DOMoveX(-2f, 2f).SetRelative(true).SetLink(gameObject);
            yield return new WaitForSeconds(0.25f);
            blackScreen.Show();
            yield return new WaitForSeconds(1f);
            SwitchToCamera(_endCam.gameObject);
            
            _layAnimApplier.Apply();
            mainCharacterView.transform.position = _endCharacterPoint.position;
            mainCharacterView.transform.rotation = _endCharacterPoint.rotation;
            _firstTile.SetActive(true);
            _helperCutsceneGO.SetActive(false);
            _raft.SetActive(false);
            RenderSettings.skybox = skyboxMat;
            _waterNight.SetActive(false);
            _waterDay.SetActive(true);
            mainCharacterView.gameObject.SetActive(true);
            Deactivate();
            _rainSound.Stop();
            _tornadoSound.Stop();
            startWoodIsland.Activate();
            
            yield return new WaitForSeconds(2f);
            blackScreen.Hide();
            foreach (var audio in _gameplayAudios)
            {
                audio.Play();
            }
            yield return new WaitForSeconds(0.25f);
            topTextHint.ShowHint("Unknown island");
            TurnOffCurrentCamera();
            yield return new WaitForSeconds(1.5f);
            
            menuBlockOverlay.Deactivate(this);
            tutorialUIMenu.Show();

            interactorCharactersCollection.mainCharacterView.GetModule<CharacterMovingModule>().onStartMoving.Once(() =>
            {
                OnEndScene();
                _layStopAnimApplier.Apply();
                tutorialUIMenu.Hide();
                buyObjectsManager.SetInBuyModeCurrentObject();
            });

            _watchedCutsceneIslandArriveSaveProperty.value = true;
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