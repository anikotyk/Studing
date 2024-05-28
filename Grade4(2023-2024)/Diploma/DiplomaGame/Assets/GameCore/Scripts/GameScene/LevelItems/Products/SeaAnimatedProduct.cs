using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameCore.GameScene.LevelItems.Products
{
    public class SeaAnimatedProduct : SellProduct
    {
        private Tween _rotateTween;
        private Tween _moveYTween;
        private Tween _moveTween;
        private float _timeRotateAroundYAxis = 60f;
        private float _timeMoveYAxis = 30f;

        private bool _isDestroyEnabled = false;

        private bool _isSeaAnimStarted = false;
        public bool isSeaAnimStarted => _isSeaAnimStarted;
        private bool _isMovingForward = false;
        public bool isMovingForward => _isMovingForward;
        
        public void StartSeaIdleAnim(bool isToMove = true)
        {
            if(_isSeaAnimStarted) return;
            _isMovingForward = isToMove;
            StartSeaIdleAnimWithoutMove();
            if (isToMove)
            {
                _moveTween = transform.DOMove(new Vector3(-1, 0, -2), 16f).SetRelative(true).SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Incremental).SetLink(gameObject);
            }
        }
        
        private void StartSeaIdleAnimWithoutMove()
        {
            _isSeaAnimStarted = true;
            _isDestroyEnabled = true;
            if (TryGetComponent<Collider>(out Collider collider))
            {
                collider.enabled = true;
            }
            int side = Random.Range(0, 2);
            side = side > 0 ? 1 : -1;
            _rotateTween = transform
                .DOLocalRotate(new Vector3(0, 360 * side, 0), _timeRotateAroundYAxis, RotateMode.FastBeyond360)
                .SetRelative(true)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetLink(gameObject);
            _moveYTween = transform.DOLocalMoveY(0.05f, _timeMoveYAxis).SetRelative(true)
                .SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        }
        
        public void EndAnims()
        {
            if (TryGetComponent<Collider>(out Collider collider))
            {
                collider.enabled = false;
            }
            
            _isDestroyEnabled = false;
            _isSeaAnimStarted = false;
            
            if (_rotateTween != null)
            {
                _rotateTween.Kill();
            }
            
            if (_moveYTween != null)
            {
                _moveYTween.Kill();
            }
            
            if (_moveTween != null)
            {
                _moveTween.Kill();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(isInCarrier) return;
            if(!_isDestroyEnabled) return;

            int layerRaft = LayerMask.NameToLayer("GroundCollider");
            if (other.GetComponent<Borders>() || other.gameObject.layer == layerRaft)
            {
                EndAnims();
                TurnOffInteractItem();
                transform.DOScale(Vector3.one*0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    Release();
                }).SetLink(gameObject);
            }
        }
    }
}