using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

namespace GameCore.Common.Misc
{
    public class FootprintsEffect : MonoBehaviour
    {
        [SerializeField] private int _poolSize;
        [SerializeField] private Sprite _footprint;
        [SerializeField] private Vector3 _startRotation;
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Transform _footPointLeft;
        [SerializeField] private Transform _footPointRight;
        [SerializeField] private float _lifeTime;
        [SerializeField] private float _zoomTime;
        [SerializeField] private Color _color;
        [SerializeField] private Transform _container;

        private List<Footprint> _footprints = new List<Footprint>();

        private List<Footprint> Footprints
        {
            get
            {
                if (_footprints.Count == 0)
                {
                    for (int i = 0; i < _poolSize; i++)
                    {
                        _footprints.Add(new Footprint(_footprint, _startScale, _startRotation, _color, _container));
                    }
                }

                return _footprints;
            }
        }

        private int _currentFootprintIndex = 0;

        private Vector2 _crossedVector;

        private void ShowFootprint(Transform target)
        {
            Footprints[_currentFootprintIndex].Show(target, _lifeTime, _zoomTime);
            _currentFootprintIndex++;
            if (_currentFootprintIndex >= Footprints.Count)
                _currentFootprintIndex = 0;
        }

        public void ShowLeftFootprint()
        {
            ShowFootprint(_footPointLeft);
        }

        public void ShowRightFootprint()
        {
            ShowFootprint(_footPointRight);
        }

        private class Footprint
        {
            private Vector3 _startScale;
            private Quaternion _startRotation;
            private SpriteRenderer _spriteRenderer;

            public Footprint(Sprite footprint, Vector3 startScale, Vector3 startRotation, Color color,
                Transform container)
            {
                var footprintObject = new GameObject();
                _spriteRenderer = footprintObject.AddComponent<SpriteRenderer>();
                _spriteRenderer.sprite = footprint;
                _spriteRenderer.color = color;
                _startScale = startScale;
                _startRotation = Quaternion.Euler(startRotation);
                footprintObject.transform.SetParent(container);
                ResetTransform();
                footprintObject.SetActive(false);
            }

            private void ResetTransform()
            {
                var footprintObject = _spriteRenderer.gameObject;
                footprintObject.transform.localScale = _startScale;
                footprintObject.transform.rotation = _startRotation;
            }

            private void Hide(float zoomTime)
            {
                DOTween.Kill(this);
                _spriteRenderer.transform.DOScale(Vector3.zero, zoomTime)
                    .OnComplete(() => _spriteRenderer.gameObject.SetActive(false)).SetLink(_spriteRenderer.gameObject).SetId(this);
            }

            public void Show(Transform followTransform, float lifeTime, float zoomTime)
            {
                DOTween.Kill(this);
                var transform = _spriteRenderer.transform;
                transform.position = followTransform.position;
                transform.localScale = Vector3.zero;
                _spriteRenderer.gameObject.SetActive(true);

                transform.DOScale(_startScale, zoomTime).SetEase(Ease.OutBack).SetLink(_spriteRenderer.gameObject).SetId(this);
                DOVirtual.DelayedCall(lifeTime, () => Hide(zoomTime), false).SetLink(_spriteRenderer.gameObject).SetId(this);
            }
        }
    }
}