using DG.Tweening;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI
{
    public class DangerDialogWindow : UIDialogWindow
    {
        [SerializeField] private Image _dangerBackImage;
        [SerializeField] private Transform _textToScale;

        public override bool playSoundOnShowAndHide => false;
        
        private Tween _scaleTween;
        private Tween _fadeTween;
        
        protected override void Init()
        {
            base.Init();
            _scaleTween = _textToScale.transform.DOScale(Vector3.one * 1.1f, 0.35f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
                .SetLink(gameObject);
            
            _fadeTween = _dangerBackImage.DOFade(0.4f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad);
        }

        public override void Hide()
        {
            if(_scaleTween!=null) _scaleTween.Kill();
            if(_fadeTween!=null) _fadeTween.Kill();
            
            base.Hide();
        }
    }
}