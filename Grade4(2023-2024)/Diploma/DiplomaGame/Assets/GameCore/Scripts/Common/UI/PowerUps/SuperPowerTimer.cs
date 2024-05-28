using System;
using DG.Tweening;
using GameCore.Common.Controllers.PowerUps;
using GameBasicsCore.Game.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI.PowerUps
{
    public class SuperPowerTimer : InjCoreMonoBehaviour
    {
        [SerializeField] private GameObject _timerIconGO;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private CanvasGroup _pauseIcon;
        [SerializeField] private Image _icon;
        
        private Tween _pauseTween;
        private Tween _pauseIdleTween;

        public void Init(ITimerablePowerUpController timerablePowerUpController, Sprite sprite)
        {
            timerablePowerUpController.onSuperPowerEffectStart.On(Appear);
            
            timerablePowerUpController.onSuperPowerWaitEffectEndUpdate.On((timeBeforeEnd) =>
            {
                UpdateTimerText(timeBeforeEnd);
            });
            
            timerablePowerUpController.onSuperPowerEffectEnd.On(Disappear);

            timerablePowerUpController.onSuperPowerPause.On(TurnOnPause);
            timerablePowerUpController.onSuperPowerUnpause.On(TurnOffPause);

            _icon.sprite = sprite;
            
            gameObject.SetActive(false);
        }

        public void UpdateTimerText(float timeBeforeEnd)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeBeforeEnd);
            _timerText.text = timeSpan.ToString(@"mm\:ss");
        }

        private void Appear()
        {
            gameObject.SetActive(true);
            _timerIconGO.transform.localScale = Vector3.zero;
            _timerIconGO.transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.InOutBack).SetLink(_timerIconGO.gameObject);
        }

        private void Disappear()
        {
            _timerIconGO.transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).SetLink(_timerIconGO.gameObject);
        }

        private void TurnOnPause()
        {
            if (_pauseTween != null)
            {
                _pauseTween.Kill();
            }
            _pauseIcon.gameObject.SetActive(true);
            _pauseIcon.alpha = 0;
            _pauseTween = _pauseIcon.DOFade(1, 0.35f).SetLink(_pauseIcon.gameObject).OnComplete(StartPauseIdle);
        }

        private void TurnOffPause()
        {
            if (_pauseIdleTween != null)
            {
                _pauseIdleTween.Kill();
            }
            if (_pauseTween != null)
            {
                _pauseTween.Kill();
            }
            _pauseTween = _pauseIcon.DOFade(0, 0.35f).OnComplete(() =>
            {
                _pauseIcon.gameObject.SetActive(false);
            }).SetLink(_pauseIcon.gameObject);
        }

        private void StartPauseIdle()
        {
            if (_pauseIdleTween != null)
            {
                _pauseIdleTween.Kill();
            }
            _pauseIdleTween = _pauseIcon.DOFade(0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetLink(_pauseIcon.gameObject);
        }
    }
}
