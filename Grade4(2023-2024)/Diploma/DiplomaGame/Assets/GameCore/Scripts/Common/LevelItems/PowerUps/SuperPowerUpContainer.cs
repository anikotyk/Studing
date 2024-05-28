using DG.Tweening;
using UnityEngine;

namespace GameCore.Common.LevelItems.PowerUps
{
    public class SuperPowerUpContainer : PowerUpContainer
    {
        [SerializeField] private Transform _visualModel;
       
        private Tween _showTween;
        private Tween _hideTween;
        private Tween _rotateTween;
        private Tween _moveTween;

        public override void Show()
        {
            base.Show();
            transform.localScale = Vector3.zero;
            _showTween = transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack).SetLink(gameObject);  
        }
        
        public override void Hide()
        {
            StopTweens();
            DisableInteractions();
            canvas.gameObject.SetActive(false);
            _hideTween = transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InBack).OnComplete(()=>
            {
                DeactivateObject();
            }).SetLink(gameObject);
        }

        public override void Reset()
        {
            base.Reset();
            
            StopTweens();
            
            InfiniteMove(_visualModel.transform);
        }
        
        public void StopTweens()
        {
            if(_showTween != null) _showTween.Kill();
            if(_hideTween != null) _hideTween.Kill();
            if(_rotateTween != null) _rotateTween.Kill();
            if(_moveTween != null) _moveTween.Kill();
        }
        
        private void InfiniteMove(Transform obj)
        {
            _moveTween = obj.DOLocalMoveY(0.1f, 1.5f)
                .SetLink(obj.gameObject)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetId(this)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
