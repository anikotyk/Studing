using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.LevelItems.Character.CharacterTyping;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.Common.Saves;
using GameCore.Common.UI;
using GameCore.GameScene.UI;
using GameCore.ShipScene.Battle;
using JetBrains.Annotations;
using GameBasicsCore.Game.API.Analytics;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Cutscenes
{
    public class ShipCompletedCutscene : Cutscene
    {
        [Inject, UsedImplicitly] public LevelEventsManager levelEventsManager { get; }
        [Inject, UsedImplicitly] public CharacterTypeSaveData characterTypeSaveData { get; }
        [Inject, UsedImplicitly] public BattleController battleController { get; }
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }

        [SerializeField] private GameObject _cutsceneContainer;
        [SerializeField] private Transform _characterHelper;
        [SerializeField] private Transform _characterSon;
        [SerializeField] private Transform _characterTribeMan;

        [SerializeField] private Transform _helperComePoint;
        [SerializeField] private Transform _sonComePoint;
        [SerializeField] private Transform _tribeManComePoint;
        
        [SerializeField] private AnimatorParameterApplier _characterSonWalkAnim;
        [SerializeField] private AnimatorParameterApplier _characterTribeManWalkAnim;
        [SerializeField] private AnimatorParameterApplier _characterSonDanceAnim;
        [SerializeField] private AnimatorParameterApplier _characterTribeManDanceAnim;
        
        [SerializeField] private AnimatorParameterApplier _characterHelperWalkAnim;
        [SerializeField] private AnimatorParameterApplier _characterHelperHugAnim;
        [SerializeField] private AnimatorParameterApplier _characterHugAnim;
        [SerializeField] private GameObject _vfxHugs;
        [SerializeField] private AnimatorParameterApplier _characterReadyAnim;
        [SerializeField] private AnimatorParameterApplier _characterHelperReadyAnim;
        [SerializeField] private CinemachineVirtualCamera _helperCam;
        [SerializeField] private CinemachineVirtualCamera _helperCloserCam;
        
        [SerializeField] private GameObject[] _deactivateObjects;

        [SerializeField] private Sprite _helperGirlSprite;
        [SerializeField] private Sprite _helperBoySprite;
        
        protected override bool deactivateMainCharacter => false;
        
        public readonly TheSignal onCutsceneCompleted = new();

        public override void Construct()
        {
            base.Construct();
            
            battleController.wavesEnded.On(StartCutscene);
            if(battleController.nextWave == null)
                StartCutscene();
        }

        public override void StartCutscene()
        {
            OnStartScene();
            DOVirtual.DelayedCall(2f, () =>
            {
                windowFactory.Create<UIDialogWindow>(CommStr.BlackScreen, (window) =>
                {
                    window.Show();
                    window.onShowComplete.Once((_) =>
                    {
                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                            window.Hide();
                        } , false).SetLink(gameObject);
                        popUpsController.containerUnderMenu.gameObject.SetActive(false);
                        _cutsceneContainer.SetActive(true);
                        SwitchToCamera(_helperCam);
                        foreach (var obj in _deactivateObjects)
                        {
                            obj.SetActive(false);
                        }
                        spawnProductsManager.container.gameObject.SetActive(false);
                    });
                    window.onHideComplete.Once((_) =>
                    {
                        StartCoroutine(CutsceneCoroutine());
                    });
                });
            }, false).SetLink(gameObject);
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            float moveTime = 4f;
            float helperMoveTime = 3f;
           
            yield return new WaitForSeconds(0.5f);

            _characterSonWalkAnim.Apply();
            _characterSon.DOMove(_sonComePoint.position, moveTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                _characterSon.DORotate(_sonComePoint.rotation.eulerAngles, 0.5f).SetLink(gameObject);
                _characterSonDanceAnim.Apply();
            }).SetLink(gameObject);
            
            _characterTribeManWalkAnim.Apply();
            _characterTribeMan.DOMove(_tribeManComePoint.position, moveTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                _characterTribeMan.DORotate(_tribeManComePoint.rotation.eulerAngles, 0.5f).SetLink(gameObject);
                _characterTribeManDanceAnim.Apply();
            }).SetLink(gameObject);
            
             
            _characterHelperWalkAnim.Apply();
            _characterHelper.DOMove(_helperComePoint.position, helperMoveTime).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(helperMoveTime);
            SwitchToCamera(_helperCloserCam);
            _characterHelperHugAnim.Apply();
            _characterHugAnim.Apply();
            _vfxHugs.SetActive(true);
            yield return new WaitForSeconds(2f);

            windowFactory.Create<CharacterReplicaDialog>(CommStr.CharacterReplicaDialog, (window) =>
            {
                var sprite = characterTypeSaveData.value.type == CharacterType.Type.boy
                    ? _helperGirlSprite
                    : _helperBoySprite;
                window.Initialize(sprite, "Through the battles, our love remains undefeated");
                window.Show();
                window.onHideComplete.Once((_) =>
                {
                    windowFactory.Create<ChapterCompleteDialog>("ChapterComplete", window =>
                    {
                        window.SetChapterNumber(4);
                        window.Show();
                    });
                    levelEventsManager.OnLevelComplete();
                    _characterReadyAnim.Apply();
                    _characterHelperReadyAnim.Apply();
                    DOVirtual.DelayedCall(2f, onCutsceneCompleted.Dispatch, false).SetLink(gameObject);
                });
            });
        }
    }
}
