using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Character.CharacterTyping;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.Common.Saves;
using GameCore.Common.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class SaveHelperCutsceneManager : Cutscene
    {
        [Inject, UsedImplicitly] public CharacterTypeSaveData characterTypeSaveData { get; }
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }

        [SerializeField] private BuyObject _activateCutsceneBuyObject;
        [SerializeField] private GameObject _boatCam;
        [SerializeField] private GameObject _boatCloserCam;
        [SerializeField] private GameObject _helperSittingCam;
        [SerializeField] private GameObject _realBoat;
        [SerializeField] private GameObject _boat;
        [SerializeField] private Transform _movePoint;
        [SerializeField] private Transform _moveBackPoint;
        [SerializeField] private GameObject _helper;
        [SerializeField] private GameObject _helperInBoat;
        [SerializeField] private AnimatorParameterApplier _characterPaddling;
        [SerializeField] private AnimatorParameterApplier _characterStopPaddling;
        [SerializeField] private AnimatorParameterApplier _helperSwim;

        [SerializeField] private Sprite _helperGirlSprite;
        [SerializeField] private Sprite _helperBoySprite;
        [SerializeField] private string _helperTextOnBoat;
        [SerializeField] private string _helperTextNearBonfire;

        public override void Construct()
        {
            base.Construct();
            if (_activateCutsceneBuyObject.GetComponent<IWindowShowable>() != null)
            {
                _activateCutsceneBuyObject.GetComponent<IWindowShowable>().onWindowClosed.Once(StartCutscene);
            }
            else
            {
                _activateCutsceneBuyObject.onBuy.Once(StartCutscene);
            }
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            _helper.SetActive(true);
            _helperSwim.Apply();
            yield return new WaitForSeconds(0.5f);
            _realBoat.SetActive(false);
            SwitchToCamera(_boatCam);
            yield return new WaitForSeconds(2f);
            _boat.SetActive(true);
            _boat.transform.localScale = Vector3.zero;
            _boat.transform.DOScale(Vector3.one, 0.5f).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
            _characterPaddling.Apply();
            _boat.transform.DOMove(_movePoint.position, 4f).SetLink(gameObject);
            yield return new WaitForSeconds(4f);
            _characterStopPaddling.Apply();
            _helper.transform.DOScale(Vector3.zero, 0.5f).SetLink(gameObject);
            _helperInBoat.SetActive(true);
            _helperInBoat.transform.localScale = Vector3.zero;
            _helperInBoat.transform.DOScale(Vector3.one, 0.5f);
            yield return new WaitForSeconds(0.5f);
            SwitchToCamera(_boatCloserCam);
            windowFactory.Create<CharacterReplicaDialog>(CommStr.CharacterReplicaDialog, (window) =>
            {
                var sprite = characterTypeSaveData.value.type == CharacterType.Type.boy ? _helperGirlSprite : _helperBoySprite;
                window.Initialize(sprite, _helperTextOnBoat);
                window.Show();
                window.onHideComplete.Once((_) =>
                {
                    _boat.transform.forward *= -1f;
                    _characterPaddling.Apply();
                    _boat.transform.DOMove(_moveBackPoint.position, 4f).SetEase(Ease.Linear).SetLink(gameObject);
                    DOVirtual.DelayedCall(1.5f, () =>
                    {
                        windowFactory.Create<UIDialogWindow>("BlackScreen", (window) =>
                        {
                            window.Show();
                            window.onShowComplete.Once((_) =>
                            {
                                _boat.SetActive(false);
                                _realBoat.SetActive(true);
                                window.Hide();
                                SwitchToCamera(_helperSittingCam);
                                DOVirtual.DelayedCall(2f, () =>
                                {
                                    windowFactory.Create<CharacterReplicaDialog>(CommStr.CharacterReplicaDialog, (window2) =>
                                    {
                                            window2.Initialize(sprite, _helperTextNearBonfire);
                                            window2.Show();
                                            window2.onHideComplete.Once((_) =>
                                            {
                                                TurnOffCurrentCamera();
                                                buyObjectsManager.ShowCurrentBuyObject();
                                                OnEndScene();
                                            });
                                        });
                                }, false).SetLink(gameObject);
                            });
                        });
                    }, false).SetLink(gameObject);
                });
            });
        }
    }
}