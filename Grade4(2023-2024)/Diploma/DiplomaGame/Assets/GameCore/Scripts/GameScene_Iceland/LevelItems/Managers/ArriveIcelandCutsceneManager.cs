using System.Collections;
using Cinemachine;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class ArriveIcelandCutsceneManager : Cutscene
    {
        [Inject, UsedImplicitly] public TutorialUIMenuWindow tutorialUIMenu { get; }
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }

        [SerializeField] private CinemachineVirtualCamera _characterTopCam;
        [SerializeField] private CinemachineVirtualCamera _northLightCam;
        [SerializeField] private TitanicCutscene _titanicCutscene;
        [SerializeField] private AnimatorParameterApplier _layAnimApplier;
        [SerializeField] private AnimatorParameterApplier _layStopAnimApplier;
        [SerializeField] private GameObject _vfxs;
        [SerializeField] private GameObject _vfxExplosion;
        [SerializeField] private AudioSource[] _turnOffMusic;
        
        protected override bool deactivateMainCharacter => false;
        
        private TheSaveProperty<int> _activeBuyObjectIndexIcelandSaveProperty;
        private TheSaveProperty<bool> _watchedCutsceneIcelandArriveSaveProperty;

        private void Awake()
        {
            _watchedCutsceneIcelandArriveSaveProperty = new(CommStr.WatchedCutsceneArrive_Iceland, linkToDispose: gameObject);
            _activeBuyObjectIndexIcelandSaveProperty = new(CommStr.ActiveBuyObjectIndex_Iceland);
            
            if (!_watchedCutsceneIcelandArriveSaveProperty.value && _activeBuyObjectIndexIcelandSaveProperty.value <= 0)
            {
                _titanicCutscene.PreSetCutscene();
                foreach (var music in _turnOffMusic)
                {
                    music.Stop();
                }
            }
        }

        public override void Construct()
        {
            base.Construct();

            if (!_watchedCutsceneIcelandArriveSaveProperty.value && _activeBuyObjectIndexIcelandSaveProperty.value <= 0)
            {
                initializeInOrderController.Add(ShowCutscene, 10000);
            }
        }

        public void ShowCutscene()
        {
            StartCoroutine(CutsceneCoroutine());
        }

        private IEnumerator CutsceneCoroutine()
        {
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);
            gameplayUIMenuWindow.gameObject.SetActive(false);
            gameplayUIMenuWindow.Hide();
            tutorialArrow3D.Disable();
            menuBlockOverlay.Activate(this);
            mainCharacterView.gameObject.SetActive(false);
            _layAnimApplier.Apply();
            _vfxs.SetActive(true);
            foreach (var music in _turnOffMusic)
            {
                music.Stop();
            }
            yield return new WaitForSeconds(0.5f);
           
            _titanicCutscene.StartCutscene();
            _titanicCutscene.onEndedScene.Once(() =>
            {
                StartCoroutine(ShowIcelandCoroutine());
            });
        }

        private IEnumerator ShowIcelandCoroutine()
        {
            mainCharacterView.gameObject.SetActive(true);
            SwitchToCamera(_characterTopCam.gameObject);
            foreach (var music in _turnOffMusic)
            {
                music.Play();
            }
            yield return new WaitForSeconds(2f);
            topTextHint.ShowHint("Day starting not so easy");
            yield return new WaitForSeconds(2f);
            SwitchToCamera(_northLightCam.gameObject);
            yield return new WaitForSeconds(2f);
            _vfxExplosion.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            topTextHint.ShowHint("The beauty of the north");
            yield return new WaitForSeconds(2f);
            TurnOffCurrentCamera();
            yield return new WaitForSeconds(2f);
            _vfxs.SetActive(false);

            menuBlockOverlay.Deactivate(this);
            tutorialUIMenu.Show();
            
            interactorCharactersCollection.mainCharacterView.GetModule<CharacterMovingModule>().onStartMoving.Once(() =>
            {
                gameplayUIMenuWindow.Show();
                _layStopAnimApplier.Apply();
                tutorialUIMenu.Hide();
                buyObjectsManager.SetInBuyModeCurrentObject();
                tutorialArrow3D.Enable();
                popUpsController.containerUnderMenu.gameObject.SetActive(true);
                popUpsController.containerOverWindow.gameObject.SetActive(true);
            });

            _watchedCutsceneIcelandArriveSaveProperty.value = true;
        }
    }
}