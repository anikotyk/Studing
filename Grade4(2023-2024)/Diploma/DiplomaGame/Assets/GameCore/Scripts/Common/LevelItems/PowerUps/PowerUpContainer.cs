using System;
using DG.Tweening;
using GameCore.Common.Controllers.PowerUps;
using GameCore.Common.UI;
using GameCore.Common.UI.PowerUps;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.Extenstions;
using GameBasicsSignals;
using TMPro;
using UnityEngine;

namespace GameCore.Common.LevelItems.PowerUps
{
    public class PowerUpContainer : MonoBehaviour
    { 
        [SerializeField] private PowerUpInteractItem _interactItem;
        public PowerUpInteractItem interactItem => _interactItem;
        [SerializeField] private Transform _timerPoint;
        public Transform timerPoint => _timerPoint;
        [SerializeField] private Transform _getPopUpPoint;
        [SerializeField] private TextMeshProUGUI _textOnModel;
        [SerializeField] private Canvas _canvas;

        public virtual bool isToUseGetPopUp => true;
        public Canvas canvas => _canvas;
        private PowerUpTimerPopUp _timerPopUp;
        private GetBubblePopUpView _getPopUp;
        public GetBubblePopUpView getPopUp => _getPopUp;

        private bool _needTurnOnGetPopUp;

        private Tween _getPopUpTween;
        private Tween _timerPopUpTween;

        protected bool isClaimed;
        
        public TheSignal onRequestWindowShow { get; } = new();
        public TheSignal onGetClicked { get; } = new();
        public TheSignal onCharacterEntered { get; } = new();
        public TheSignal onCharacterExited { get; } = new();
        public TheSignal onClaimed { get; } = new();
        
        private void Awake()
        {
            if (_canvas) _canvas.gameObject.SetActive(false);
        }

        public virtual void Init(Vector3 position, PopUpsController popUpsController, PowerUpController controller)
        {
            isClaimed = false;
            transform.position = position;
            SetupGetPopUp(popUpsController);
            SetupTimer(popUpsController, controller);
        }

        private void SetupGetPopUp(PopUpsController popUpsController)
        {
            if (_getPopUp) return;
            _getPopUp = SpawnGetPopUp(popUpsController);
            _getPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
            _getPopUp.worldSpaceConverter.followWorldObject = _getPopUpPoint;
            _getPopUp.transform.localScale = Vector3.zero;
            _getPopUp.gameObject.SetActive(false);
            _getPopUp.onClick.On(()=>
            { 
                onRequestWindowShow.Dispatch();
                OnRequestedWindowShow();
                onGetClicked.Dispatch();
            });
        }

        protected virtual GetBubblePopUpView SpawnGetPopUp(PopUpsController popUpsController)
        {
            return popUpsController.SpawnUnderMenu<GetBubblePopUpView>();
        }

        public void SetTextOnModel(string text)
        {
            if (!_textOnModel) return;
            
            if (_canvas) _canvas.gameObject.SetActive(true); 
            _textOnModel.text = text;
        }
        
        public virtual void OnCharacterExited()
        {
            onCharacterExited.Dispatch();
            if (!isToUseGetPopUp) return;
            TurnOffGetPopUp();
        }
        
        public virtual void OnRequestedWindowShow()
        {
           
        }
        
        public void TurnOffGetPopUp()
        {
            if(!_getPopUp) return;

            _needTurnOnGetPopUp = _getPopUp.gameObject.activeSelf;
            //can be changed to _getPopUpTween.DisappearAnimation() if object isn't reusable
            if(_getPopUpTween != null) _getPopUpTween.Kill();
            _getPopUpTween = _getPopUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _getPopUp.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }

        public virtual void OnCharacterEntered()
        {
            onCharacterEntered.Dispatch();
            if (!isToUseGetPopUp) return;
            TurnOnGetPopUp();
        }
        
        private void TurnOnGetPopUp()
        {
            if(!_getPopUp) return;
            if(_getPopUpTween != null) _getPopUpTween.Kill();
            _getPopUp.gameObject.SetActive(true);
            _getPopUp.AppearAnimation();
        }
        
        private void SetupTimer(PopUpsController popUpsController, PowerUpController controller)
        {
            if (_timerPopUp) return;
            _timerPopUp = popUpsController.SpawnUnderMenu<PowerUpTimerPopUp>();
            _timerPopUp.worldSpaceConverter.followWorldObject = _timerPoint;

            controller.onPowerUpWaitDestroyUpdate.On((timeLeft) =>
            {
                TimeSpan timeUntilDisappear = TimeSpan.FromSeconds(timeLeft);
                _timerPopUp.timerText.text = timeUntilDisappear.ToString(@"mm\:ss");;
            });
            
            _timerPopUp.AppearAnimation();
        }

        public void TurnOffTimer()
        {
            if(!_timerPopUp) return;
            _timerPopUp.worldSpaceConverter.enabled = false;
            
            //can be changed to _timerPopUp.DisappearAnimation() if object isn't reusable
            if(_timerPopUpTween != null) _timerPopUpTween.Kill();
            _timerPopUpTween = _timerPopUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _timerPopUp.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }

        public void TurnOnTimer()
        {
            if(!_timerPopUp) return;
            _timerPopUp.worldSpaceConverter.enabled = true;
            if(_timerPopUpTween != null) _timerPopUpTween.Kill();
            _timerPopUp.AppearAnimation();
        }
        
        public virtual void OnClaimed()
        {
            isClaimed = true;
            onClaimed.Dispatch();
            Hide();
        }

        public virtual void Hide()
        {
            HideInternal();
        }

        public void HideInternal()
        {
            DisableInteractions();
            DeactivateObject();
        }

        protected void DisableInteractions()
        {
            interactItem.enabled = false;
            TurnOffTimer();
            TurnOffGetPopUp();
        }
        
        protected virtual void DeactivateObject()
        {
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            Reset();
        }

        public virtual void Reset()
        {
            interactItem.enabled = true;
            TurnOnTimer();
        }

        public void OnSkippedPowerUp()
        {
            Reset();
            if (_needTurnOnGetPopUp) TurnOnGetPopUp();
        }

        public void OnCharacterInteracted()
        {
            if (isToUseGetPopUp) return;
            
            onRequestWindowShow.Dispatch();
            OnRequestedWindowShow();
        }
        
        public virtual void OnCameraAttention()
        {
            
        }
        
        public virtual void OnCameraAttentionEnded()
        {
            
        }
    }
}
