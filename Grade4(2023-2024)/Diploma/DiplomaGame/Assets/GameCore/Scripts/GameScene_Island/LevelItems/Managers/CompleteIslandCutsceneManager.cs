using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Managers;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Managers
{
    public class CompleteIslandCutsceneManager : Cutscene
    {
        [Inject, UsedImplicitly] public SceneLoader sceneLoader { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        [SerializeField] private BuyObject _buyObjectActivate;
        
        [SerializeField] private Transform _character;
        [SerializeField] private Transform _tribeMan;
        
        [SerializeField] private AnimatorParameterApplier _characterHands;
        [SerializeField] private AnimatorParameterApplier _characterMove;
        [SerializeField] private AnimatorParameterApplier _characterNotMove;
        
        [SerializeField] private AnimatorParameterApplier _tribeManHandMove;
        [SerializeField] private AnimatorParameterApplier _tribeManMove;
        [SerializeField] private AnimatorParameterApplier _tribeManStopMove;
        [SerializeField] private AnimatorParameterApplier _tribeManSit;
        [SerializeField] private AnimatorParameterApplier _tribeManEats;
        
        [SerializeField] private Transform _targetPoint1;
        [SerializeField] private Transform _targetPoint2;
        [SerializeField] private Transform _targetPoint3;

        [SerializeField] private Transform _targetPointTribeMan1;
        [SerializeField] private Transform _targetPointTribeMan2;
        
        [SerializeField] private Transform _cheese;
        [SerializeField] private Transform _cheesePoint;
        [SerializeField] private Transform _cheesePoint2;
        
        [SerializeField] private GameObject _camCharacter1;
        [SerializeField] private GameObject _camCharacter2;
        [SerializeField] private GameObject _camTribeMan1;
        [SerializeField] private GameObject _camCharacterClose;
        [SerializeField] private GameObject _camPart2;
        
        [SerializeField] private GameObject _vfxMainCharacter;
        
        [SerializeField] private GameObject _part1;
        [SerializeField] private GameObject _part2;
        
        [SerializeField] private Transform _movable;
        [SerializeField] private GameObject[] _activateObjects;
        [SerializeField] private GameObject[] _deactivateObjects;
        [SerializeField] private AudioSource[] _globalMusic;

        protected override bool deactivateMainCharacter => false;
        
        public override void Construct()
        {
            base.Construct();
            
            _buyObjectActivate.onBuy.Once(() =>
            {
                StartCutscene();
            });
            
            initializeInOrderController.Add(Initialize, 3000);
        }

        private void Initialize()
        {
            if (_buyObjectActivate.isBought) StartCutscene();
        }
        
        private void Activate()
        {
            foreach (var obj in _activateObjects)
            {
                obj.SetActive(true);
            }

            foreach (var obj in _deactivateObjects)
            {
                obj.SetActive(false);
            }
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            Activate();
            
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);
            _characterHands.Apply();
            
            _part1.gameObject.SetActive(true);
            SwitchToCamera(_camCharacter1);
            yield return new WaitForSeconds(2f);
            _characterMove.Apply();
            _character.DOMove(_targetPoint1.position, 2f).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(2f);
            _characterNotMove.Apply();
            _characterHands.ReleaseLayerWeight();
            _cheese.SetParent(null);
            _cheese.DOJump(_cheesePoint.position, 1f, 1, 0.5f).SetLink(gameObject);
            _cheese.DORotate(Vector3.zero, 0.5f).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
            _character.DORotate(_targetPoint2.rotation.eulerAngles, 0.5f).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(0.4f);
            SwitchToCamera(_camCharacter2);
            _tribeMan.gameObject.SetActive(true);
            _tribeManHandMove.Apply();
            yield return new WaitForSeconds(0.75f);
            _character.DOMove(_targetPoint3.position, 0.5f).SetEase(Ease.Linear).SetLink(gameObject);
            _character.DORotate(_targetPoint3.rotation.eulerAngles, 0.5f).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
            SwitchToCamera(_camTribeMan1);
            yield return new WaitForSeconds(1.5f);
            _tribeManMove.Apply();
            _tribeMan.DOMove(_targetPointTribeMan1.position, 0.5f).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
            _tribeManStopMove.Apply();
            _tribeMan.DORotate(_targetPointTribeMan2.rotation.eulerAngles, 0.5f).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(0.65f);
            _tribeManSit.Apply();
            _cheese.SetParent(_cheesePoint2);
            _cheese.DOLocalJump(Vector3.zero,1f, 1, 0.5f).SetLink(gameObject);
            yield return new WaitForSeconds(0.25f);
            _tribeManEats.Apply();
            yield return new WaitForSeconds(1.5f);
            SwitchToCamera(_camCharacterClose);
            yield return new WaitForSeconds(1f);
            _vfxMainCharacter.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            foreach (var music in _globalMusic)
            {
                music.Stop();
            }
            windowFactory.Create<UIWindow>("BlackScreen", window =>
            {
                window.Show();
                window.onShowComplete.Once((_) =>
                {
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        _part1.gameObject.SetActive(false);
                        _part2.gameObject.SetActive(true);
                        SwitchToCamera(_camPart2);
                        window.Hide();
                    }, false).SetLink(gameObject);
                });
            });
            
            yield return new WaitForSeconds(2.5f);
            windowFactory.Create<ChapterCompleteDialog>("ChapterComplete", window =>
            {
                window.SetChapterNumber(3);
                window.Show();
            });
            _movable.DOMove(_movable.forward, 1f).SetRelative(true).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(4f);
            
            windowFactory.Create<UIWindow>("BlackScreen", window =>
            {
                window.Show();
                window.onShowComplete.Once((_) =>
                {
                    DOVirtual.DelayedCall(0.1f, () =>
                    {
                        sceneLoader.Load(CommStr.GameScene_Iceland);
                    }, false).SetLink(gameObject);
                });
            });
        }
    }
}