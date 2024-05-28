using DG.Tweening;
using GameCore.Common.Settings;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Tools.DOTweenAnimations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI.PowerUps
{
    public class TimerOfferButton : InjCoreMonoBehaviour
    {
        [SerializeField] private Button _btn;
        [SerializeField] private GameObject _iconGO;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _timerText;
        protected TextMeshProUGUI timerText => _timerText;
        [SerializeField] private Image _offerBackground;
        [SerializeField] private Color _dangerColor;
        
        private Tween _pulseTween;
        private bool _isInWarningMode = false;
        protected bool isInWarningMode => _isInWarningMode;
        private Color _offerBackgroundColor;
        private Tween _colorTween;

        protected void Init()
        {
            _btn.onClick.AddListener(OnButtonClick);
            _offerBackgroundColor = _offerBackground.color;
            gameObject.SetActive(false);
        }
        
        public void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        protected void Appear()
        {
            gameObject.SetActive(true);
            _iconGO.transform.localScale = Vector3.zero;
            _iconGO.transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.InOutBack).OnComplete(()=>
            {
                int cntPulseTimes = PowerUpsSettings.def.pulseIconAfterAppearTimes;
                if (cntPulseTimes > 0)
                {
                    StartPulse(1.2f, 0.35f);
                    DOVirtual.DelayedCall(0.35f * cntPulseTimes, ()=>
                    {
                        StopPulse();
                        StartPulse();
                    }).SetLink(gameObject);
                }
                
            }).SetLink(_iconGO.gameObject);
        }

        protected void Disappear()
        {
            StopPulse();
            TurnOffWarningMode();
            _iconGO.transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).SetLink(_iconGO.gameObject);
        }

        private void StartPulse(float toScale = 1.1f, float duration = 1f)
        {
            StopPulse();
            
            _pulseTween = _icon.transform.DOPulseScaleDefault(1f, toScale, duration)
                .SetLoops(-1)
                .SetLink(gameObject);
        }
        
        private void StopPulse()
        {
            if(_pulseTween != null) _pulseTween.Kill();
        }

        protected void TurnOnWarningMode()
        { 
            if (_isInWarningMode) return;
            _isInWarningMode = true;
            StartPulse(1.2f, 0.5f);
            if (_colorTween != null) _colorTween.Kill();
            _colorTween = _offerBackground.DOColor(_dangerColor, 0.5f).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        }
        
        protected void TurnOffWarningMode()
        { 
            _isInWarningMode = false;
            StopPulse();
            if (_colorTween != null) _colorTween.Kill();
            _offerBackground.color = _offerBackgroundColor;
        }

        public virtual void OnButtonClick()
        {
            StopPulse();
            TurnOffWarningMode();
        }
    }
}
