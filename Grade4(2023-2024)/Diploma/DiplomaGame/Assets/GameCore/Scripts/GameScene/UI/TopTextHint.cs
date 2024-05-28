using DG.Tweening;
using GameBasicsCore.Game.Core;
using TMPro;
using UnityEngine;

namespace GameCore.GameScene.UI
{
    public class TopTextHint : InjCoreMonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _showTimeDefault;

        [SerializeField] private float _fadeValue = 0.6f;

        private Tween _tween;
        
        public void ShowHint(string text, float showTime = -1, float fadeValue = -1)
        {
            if (_tween != null)
            {
                _tween.Kill();
            }

            _text.text = text;
            showTime = showTime > 0 ? showTime : _showTimeDefault;
            fadeValue = fadeValue > 0 ? fadeValue : _fadeValue;
            
            _tween = _text.DOFade(fadeValue, 0.5f).OnComplete(() =>
            {
                _text.DOFade(0, 0.5f).SetDelay(showTime);
            });
        }
    }
}