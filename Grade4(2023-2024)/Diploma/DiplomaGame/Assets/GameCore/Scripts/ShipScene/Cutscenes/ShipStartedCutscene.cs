using System.Collections;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.ShipScene.Battle;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Cutscenes
{
    public class ShipStartedCutscene : Cutscene
    {
        [Inject, UsedImplicitly] public BattleController battleController { get; }
        [Inject, UsedImplicitly] public LevelEventsManager levelEventsManager { get; }
        
        [SerializeField] private GameObject _cutsceneContainer;
        [SerializeField] private GameObject _enemyShip;
        [SerializeField] private Transform _character;
        [SerializeField] private Transform _characterHelper;
        [SerializeField] private Transform _characterSon;
        [SerializeField] private Transform _characterTribeMan;

        [SerializeField] private Transform _characterRotatePoint;
        [SerializeField] private Transform _helperComePoint;
        [SerializeField] private Transform _sonComePoint;
        [SerializeField] private Transform _tribeManComePoint;
        
        [SerializeField] private AnimatorParameterApplier _characterSonRunAnim;
        [SerializeField] private AnimatorParameterApplier _characterTribeManRunAnim;
        [SerializeField] private AnimatorParameterApplier _characterHelperRunAnim;

        [SerializeField] private AnimatorParameterApplier _characterGetOutAnim;
        [SerializeField] private AnimatorParameterApplier _characterWaitButtleAnim;
        
        [SerializeField] private CinemachineVirtualCamera _characterCam;
        [SerializeField] private CinemachineVirtualCamera _helperCam;
        [SerializeField] private CinemachineVirtualCamera _shipCam;
        
        [SerializeField] private AudioSource _fightingMusic;

        [SerializeField] private GameObject _shipCanvas;
        
        protected override bool deactivateMainCharacter => false;
        
        private TheSaveProperty<bool> _watchedCutsceneArriveCached;
        private TheSaveProperty<bool> watchedCutsceneArrive =>
            _watchedCutsceneArriveCached ??=
                new TheSaveProperty<bool>(CommStr.WatchedCutsceneArrive_Ship, linkToDispose: gameObject);

        public override void Construct()
        {
            base.Construct();

            if (!watchedCutsceneArrive.value)
            {
                StartCutscene();
            }
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            SwitchToCamera(_shipCam);
            
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            _cutsceneContainer.SetActive(true);
            _shipCanvas.gameObject.SetActive(false);

            float moveTime = 2f;
            yield return new WaitForSeconds(1f);
            topTextHint.ShowHint("Night on ship");
            yield return new WaitForSeconds(3f);
            
            _enemyShip.SetActive(true);
            _fightingMusic.Play();
            yield return new WaitForSeconds(2f);
            
            yield return new WaitForSeconds(3f);
            topTextHint.ShowHint("There are no people on board");
            SwitchToCamera(_characterCam);
            yield return new WaitForSeconds(1.5f);
            
            _characterGetOutAnim.Apply();
            yield return new WaitForSeconds(0.1f);
            SwitchToCamera(_helperCam);
            yield return new WaitForSeconds(0.4f);
            
            _characterSonRunAnim.Apply();
            _characterSon.DOLookAt(_sonComePoint.position, 0.5f).OnComplete(() =>
            {
                _characterSon.DOMove(_sonComePoint.position, moveTime).SetEase(Ease.Linear).SetLink(gameObject);
            }).SetLink(gameObject);
            
            _characterTribeManRunAnim.Apply();
            _characterTribeMan.DOLookAt(_tribeManComePoint.position, 0.5f).OnComplete(() =>
            {
                _characterTribeMan.DOMove(_tribeManComePoint.position, moveTime).SetEase(Ease.Linear).SetLink(gameObject);
            }).SetLink(gameObject);


            _characterHelperRunAnim.Apply();
            _characterHelper.DOLookAt(_helperComePoint.position, 0.5f).OnComplete(() =>
            {
                _characterHelper.DOMove(_helperComePoint.position, moveTime).SetEase(Ease.Linear).SetLink(gameObject);
            }).SetLink(gameObject);
            yield return new WaitForSeconds(moveTime);
            SwitchToCamera(_characterCam);
            yield return new WaitForSeconds(1f);
            _character.DORotate(_characterRotatePoint.rotation.eulerAngles, 0.5f).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
            _characterWaitButtleAnim.Apply();
            yield return new WaitForSeconds(0.5f);
            watchedCutsceneArrive.value = true;
            watchedCutsceneArrive.Dispose();
            levelEventsManager.OnLevelStart();
            
            windowFactory.Create<UIDialogWindow>(CommStr.BlackScreen, (window) =>
            {
                window.Show();
                window.onShowComplete.Once((_) =>
                {
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        window.Hide();
                    } , false).SetLink(gameObject);
                    OnEndScene();
                    popUpsController.containerUnderMenu.gameObject.SetActive(true);
                    _cutsceneContainer.SetActive(false);
                    TurnOffCurrentCamera();
                });
                window.onHideStart.Once((_) =>
                {
                    battleController.StartBattle();
                });
                window.onHideComplete.Once((_) =>
                {
                    _shipCanvas.gameObject.SetActive(true);
                });
            });
        }
    }
}
