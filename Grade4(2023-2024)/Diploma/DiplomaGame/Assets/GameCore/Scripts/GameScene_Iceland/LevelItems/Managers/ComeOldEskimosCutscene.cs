using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.Common.UI;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class ComeOldEskimosCutscene : Cutscene
    {
        [SerializeField] private BuyObject _buyObjectActivate;
        [SerializeField] private GameObject _oldEskimosCam;
        [SerializeField] private GameObject _sonCam;
        [SerializeField] private GameObject _son;
        [SerializeField] private GameObject _oldEskimos;
        [SerializeField] private Transform _movePoint;
        [SerializeField] private AnimatorParameterApplier _walkAnim;
        [SerializeField] private AnimatorParameterApplier _endWalkAnim;
        [SerializeField] private Sprite _oldEskimosSprite;
        [SerializeField] private string _oldEskimosText;
        protected override bool deactivateMainCharacter => false;

        public override void Construct()
        {
            base.Construct();

            _buyObjectActivate.onBuy.Once(StartCutscene);
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverWindow.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            SwitchToCamera(_oldEskimosCam);
            yield return new WaitForSeconds(2f);
            _oldEskimos.SetActive(true);
            _oldEskimos.transform.localScale = Vector3.zero;
            _oldEskimos.transform.DOScale(Vector3.one, 0.5f).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
            _walkAnim.Apply();
            _oldEskimos.transform.DOMove(_movePoint.position, 4f).SetEase(Ease.Linear).SetLink(gameObject);
            yield return new WaitForSeconds(4f);
            _endWalkAnim.Apply();
            yield return new WaitForSeconds(1f);
            windowFactory.Create<CharacterReplicaDialog>(CommStr.CharacterReplicaDialog, (window) =>
            {
                window.Initialize(_oldEskimosSprite, _oldEskimosText);
                window.Show();
                window.onHideComplete.Once((_) =>
                {
                    _oldEskimos.SetActive(false);
                    _son.SetActive(true);
                    SwitchToCamera(_sonCam);
                    DOVirtual.DelayedCall(6f, () =>
                    {
                        TurnOffCurrentCamera();
                        DOVirtual.DelayedCall(2f, () =>
                        {
                            windowFactory.Create<UIWindow>("ComingSoon", window =>
                            {
                                window.Show();
                                window.onHideComplete.Once((_) =>
                                {
                                    popUpsController.containerUnderMenu.gameObject.SetActive(true);
                                    popUpsController.containerOverWindow.gameObject.SetActive(true);
                                    OnEndScene();
                                });
                            });
                        }, false).SetLink(gameObject);
                    }, false).SetLink(gameObject);
                });
            });
        }
    }
}