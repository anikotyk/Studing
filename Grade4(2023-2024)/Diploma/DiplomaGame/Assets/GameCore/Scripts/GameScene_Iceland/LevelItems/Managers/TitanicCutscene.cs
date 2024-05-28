using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class TitanicCutscene : Cutscene
    {
        [SerializeField] private GameObject _cutsceneGO;
        [SerializeField] private GameObject _part1GO;
        [SerializeField] private Transform _ship;
        [SerializeField] private GameObject _vfxHearts;
        [SerializeField] private Transform _iceberg;
        [SerializeField] private GameObject _vfxExplode;
        
        [SerializeField] private GameObject _part2GO;
        [SerializeField] private Transform _jack;
        [SerializeField] private Transform _smallRaft;
        [SerializeField] private Transform _titanic;
        [SerializeField] private AudioSource _soundSplash;
        [SerializeField] private AudioSource _backMusic;
        
        [SerializeField] private GameObject _camTitanic;
        [SerializeField] private GameObject _camTitanicShaking;
        [SerializeField] private GameObject _camRosa;
        [SerializeField] private GameObject _camJack;
        
        protected override bool deactivateGameplayUIWindow => false;
        protected override bool deactivateTutorialArrow => false;
        protected override bool activateMenuBlockOverlay => false;
        protected override bool deactivateMainCharacter => false;

        public void PreSetCutscene()
        {
            _cutsceneGO.gameObject.SetActive(true);
            _part1GO.gameObject.SetActive(true);
            SwitchToCamera(_camTitanic);
        }
        
        protected override IEnumerator CutsceneCoroutine()
        {
            _cutsceneGO.gameObject.SetActive(true);
            _backMusic.Play();
            _part1GO.gameObject.SetActive(true);
            SwitchToCamera(_camTitanic);
            Vector3 dir = _ship.forward;
            Tween moveTween = _ship.DOMove(dir, 0.25f).SetLoops(100, LoopType.Incremental).SetRelative(true).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(4f);
            _vfxHearts.SetActive(false);
            _iceberg.gameObject.SetActive(true);
            topTextHint.ShowHint("There is an iceberg!");
            _iceberg.DOMoveY(10f, 0.75f).SetRelative(true).SetEase(Ease.InOutBack, 2f).SetLink(gameObject);
            yield return new WaitForSeconds(2f);
            _vfxExplode.SetActive(true);
            moveTween.Kill();
            SwitchToCamera(_camTitanicShaking);
            windowFactory.Create<UIDialogWindow>("BlackScreen", (window) =>
            {
                window.Show();
                window.onShowComplete.Once((_) =>
                {
                    DOVirtual.DelayedCall(1.5f, () =>
                    {
                        _part1GO.gameObject.SetActive(false);
                        StartCoroutine(CutsceneOnRaftCoroutine());
                        window.Hide();
                    }).SetLink(gameObject);
                });
            });
        }

        private IEnumerator CutsceneOnRaftCoroutine()
        {
            _part2GO.SetActive(true);
            SwitchToCamera(_camRosa);
            
            yield return new WaitForSeconds(1.5f);
            SwitchToCamera(_camJack);
            yield return new WaitForSeconds(2f);
            
            _titanic.DOMoveY(-1f, 4f).SetRelative(true).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(2f);
            _jack.DOLocalMoveZ(-0.5f, 2f).SetRelative(true).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(2f);
            _titanic.DOMoveY(-10f, 2f).SetRelative(true).SetEase(Ease.InOutBack).SetLink(gameObject);
            _jack.DOMoveY(-5f, 2f).SetRelative(true).SetEase(Ease.Linear).SetLink(gameObject);
            _smallRaft.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.InOutBack);
            _smallRaft.DOLocalMoveY(0, 0.5f).SetEase(Ease.InOutBack);
            _soundSplash.Play();
            yield return new WaitForSeconds(1.5f);
            _titanic.gameObject.SetActive(false);
            _jack.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            windowFactory.Create<UIDialogWindow>("BlackScreen", (window) =>
            {
                window.Show();
                window.onShowComplete.Once((_) =>
                {
                    _backMusic.DOFade(0, 1.5f).SetLink(gameObject);
                    DOVirtual.DelayedCall(1.5f, () =>
                    {
                        _cutsceneGO.SetActive(false);
                        _backMusic.Stop();
                        window.Hide();
                    }).SetLink(gameObject);
                    TurnOffCurrentCamera();
                    OnEndScene();
                });
            });
        }
    }
}