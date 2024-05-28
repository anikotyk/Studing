using DG.Tweening;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using UnityEngine;

namespace GameCore.GameScene.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TutorialUIMenuWindow : UIMenuWindow
    {
        public bool destroyWhenHide = true;
        public float hideDuration = 0.75f;
        public Ease hideEase = Ease.OutQuad;

        private CanvasGroup _group;

        private Tween _tween;

        protected override void Init()
        {
            gameObject.SetActive(shown = _activeAtStart);
            
            _group = GetComponent<CanvasGroup>();
            _group.alpha = _activeAtStart ? 1 : 0;
        }

        public override void Show(float delay)
        {
            shown = true;
            if (_tween != null)
            {
                _tween.Kill();
            }
            _tween = _group.DOFade(1f, 0.5f)
                .SetDelay(delay)
                .OnStart(() => gameObject.SetActive(true)).SetLink(gameObject);
        }

        public override void Hide(float delay)
        {
            if (!shown) return;
            shown = false;
            
            onHideStart.Dispatch(this);
            _group.blocksRaycasts = false;
            if (_tween != null)
            {
                _tween.Kill();
            }
            _tween = _group.DOFade(0f, hideDuration)
                .SetDelay(delay)
                .SetEase(hideEase)
                .OnComplete(() =>
                {
                    onHideComplete.Dispatch(this);
                    if (destroyWhenHide)
                    {
                        Destroy(gameObject, 1);
                    }
                }).SetLink(gameObject);
        }
    }
}