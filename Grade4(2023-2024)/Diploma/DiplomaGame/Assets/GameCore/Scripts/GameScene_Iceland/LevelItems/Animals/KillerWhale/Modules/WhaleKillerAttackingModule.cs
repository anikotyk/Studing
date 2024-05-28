using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using GameCore.Common.Misc;
using GameCore.GameScene_Iceland.LevelItems.Items;
using GameCore.GameScene_Iceland.LevelItems.Managers;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Animals.KillerWhale.Modules
{
    public class WhaleKillerAttackingModule : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SwimHuntingManager swimHuntingManager { get; }
        [Inject, UsedImplicitly] public VfxStack vfxStack { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }

        [SerializeField] private DangerZone _preAttackEffect;
        [SerializeField] private AnimatorParameterApplier _attackAnim;
        [SerializeField] private AnimatorParameterApplier _diveAnim;
        [SerializeField] private CinemachineVirtualCamera _killBoatCam;
        [SerializeField] private Transform _attentionPopUpPoint;
        [SerializeField] private string _attackAnimName = "Hit";
        
        [SerializeField] private AudioSource _attackSound;
        [SerializeField] private AudioSource _killBoatSound;
        [SerializeField] private AudioSource _waterJumpSound;
        [SerializeField] private AudioSource _failSound;
        
        private WhaleKillerView _viewCached;
        public WhaleKillerView view => _viewCached ??= GetComponentInParent<WhaleKillerView>(true);
        private InteractItem _interactItem;
        public InteractItem interactItem => _interactItem ??= GetComponentInChildren<InteractItem>(true);
        private BoatHuntingView _boatView;

        private ImagePopUpView _attentionPopUp;
        private Tween _attackingTween;
        private bool _isAttacking;
        private List<Tween> _currentTweens = new List<Tween>();
        
        public void StartAttack(BoatHuntingView boatView)
        {
            if (_isAttacking) return;
            _isAttacking = true;
            interactItem.enabled = false;
            _boatView = boatView;
            
            view.walkBySplineModule.StopWalk();
            view.healthAnimalModule.ResetHealth(false);
            view.getDamageModule.DisableModule();
            SetAttentionPopUp();
            
            Quaternion targetRotation = Quaternion.LookRotation(_boatView.transform.position - view.transform.position);
            float angle = Quaternion.Angle( view.transform.rotation, targetRotation);
            float rotationSpeed = 200f;
            float time = angle / rotationSpeed;
            var tween = view.transform.DOLookAt(_boatView.transform.position, time, AxisConstraint.None, Vector3.up).OnComplete(() =>
            {
                _diveAnim.Apply();
                var tween =DOVirtual.DelayedCall(0.6f, () =>
                {
                    view.animator.gameObject.SetActive(false);
                    HideAttentionPopUp();
                    _attackingTween = DOVirtual.DelayedCall(0.5f, AppearAndAttack, false).SetLink(gameObject);
                }, false).SetLink(gameObject);
                _currentTweens.Add(tween);
            }).SetLink(gameObject);
            _currentTweens.Add(tween);
        }
        
        public void StopAttack()
        {
            if(_boatView) _boatView.swimHuntingAttackModule.EndAttack();
            _boatView = null;
            if(_attackingTween!=null)
            {
                _attackingTween.Kill();
            }
            _preAttackEffect.ExitZoneInternal();
            _preAttackEffect.gameObject.SetActive(false);
            foreach (var tween in _currentTweens)
            {
                if(tween != null) tween.Kill();
            }
            _currentTweens.Clear();
            _isAttacking = false;
        }
        
        public void LockStartAttack()
        {
            interactItem.enabled = false;
        }
        
        public void UnlockStartAttack()
        {
            interactItem.enabled = true;
        }

        public void Hide()
        {
            _boatView.swimHuntingAttackModule.EndAttack();
            view.getDamageModule.DisableModule();
            var tween = view.transform.DOMoveY(-6f, 0.35f).SetRelative(true).SetEase(Ease.Linear).OnComplete(() =>
            {
                view.animator.gameObject.SetActive(false);
            }).SetLink(gameObject);
            _currentTweens.Add(tween);
            Vector3 vfxPos = view.transform.position - Vector3.up * 5f;
            var tween2 = DOVirtual.DelayedCall(0.15f, () =>
            {
                vfxStack.Spawn(CommStr.WaterSplash, vfxPos);
                _waterJumpSound.Play();
            }, false).SetLink(gameObject);
            _currentTweens.Add(tween2);
        }

        private void AppearAndAttack()
        {
            Vector3 pos = GetAttackPosition();
            _preAttackEffect.gameObject.SetActive(true);
            _preAttackEffect.transform.position = pos;
            _preAttackEffect.ExitZoneInternal();
            
            var tween = DOVirtual.DelayedCall(2f, () =>
            {
                view.getDamageModule.EnableModule();
                _boatView.swimHuntingAttackModule.StartAttack(view);

                view.transform.position = pos - Vector3.up * 3f;
                view.animator.gameObject.SetActive(true);
                view.transform.rotation = Quaternion.Euler(-90, 0, 90);
               // _attackAnim.Apply();
                view.animator.Play(_attackAnimName, -1, 0f);
               
                if (_preAttackEffect.IsNowBoatInZone())
                {
                    KillBoat();
                }
                _preAttackEffect.ExitZoneInternal();
                _preAttackEffect.gameObject.SetActive(false);
                vfxStack.Spawn(CommStr.WaterSplash, pos);
                _waterJumpSound.Play();
               
                var tween = view.transform.DOMoveY(5f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    _attackSound.Play();
                    var tween =view.transform.DOMoveY(1f, 0.5f).SetRelative(true).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        var tween = DOVirtual.DelayedCall(0.5f, () =>
                        {
                            Hide();
                            _attackingTween = DOVirtual.DelayedCall(1f, AppearAndAttack, false).SetLink(gameObject);
                        }, false).SetLink(gameObject);
                        _currentTweens.Add(tween);
                    }).SetLink(gameObject);
                    _currentTweens.Add(tween);
                }).SetLink(gameObject);
                _currentTweens.Add(tween);
            }, false).SetLink(gameObject);
            _currentTweens.Add(tween);
        }

        private void KillBoat()
        {
            _boatView.walkerController.TurnOffMovement();
            view.getDamageModule.DisableModule();
            _boatView.swimHuntingAttackModule.EndAttack();
            _killBoatCam.gameObject.SetActive(true);
            
            var tween = _boatView.transform.DOMove(view.animator.transform.position + Vector3.up * 15f + Vector3.right * 0.5f, 0.65f).OnComplete(() =>
            {
                Time.timeScale = 0.25f;
                var tween = _boatView.transform.DOLocalRotate(new Vector3(0, -90f, 0), 0.45f).SetLink(gameObject);
                _currentTweens.Add(tween);
                var tween2 = _boatView.visual.DOLocalRotate(new Vector3(0, 90f, 90f), 0.45f).OnComplete(() =>
                {
                    Time.timeScale = 0.45f;
                }).SetLink(gameObject);
                _currentTweens.Add(tween2);
                _killBoatSound.Play();
                var tween3 = _boatView.transform.DOMoveY(-6f, 0.75f).SetRelative(true).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Time.timeScale = 1f;
                    OnBoatKilled();
                    _killBoatCam.gameObject.SetActive(false);
                }).SetLink(gameObject);
                _currentTweens.Add(tween3);
            }).SetLink(gameObject);
            _currentTweens.Add(tween);
        }

        private void OnBoatKilled()
        {
            _boatView.gameObject.SetActive(false);
            var tween = DOVirtual.DelayedCall(0.75f, () =>
            {
                windowFactory.Create<UIDialogWindow>("BlackScreen", window =>
                {
                    window.Show();
                    window.onShowComplete.Once((_) =>
                    {
                        _failSound.Play();
                        window.Hide();
                        view.ResetView();
                        swimHuntingManager.StopHunting();
                    });
                });
            }).SetLink(gameObject);
            _currentTweens.Add(tween);
        }

        private Vector3 GetAttackPosition()
        {
            Vector3 pos = _boatView.transform.position;
            return pos;
        }
        
        private void SetAttentionPopUp()
        {
            if (_attentionPopUp == null)
            {
                _attentionPopUp = popUpsController.SpawnUnderMenu<ImagePopUpView>("AttentionPopUp");
                _attentionPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
                _attentionPopUp.worldSpaceConverter.followWorldObject = _attentionPopUpPoint.transform;
            }
            
            _attentionPopUp.gameObject.SetActive(true);
            _attentionPopUp.AppearAnimation(0.5f, Ease.InOutBack);
        }
        
        private void HideAttentionPopUp()
        {
            if (_attentionPopUp != null)
            {
                _attentionPopUp.DisappearAnimation(0.25f, Ease.InOutBack); 
            }
        }
    }
}