using DG.Tweening;
using UnityEngine;

namespace GameCore.Common.LevelItems
{
    public class ManualBuyObjectVisible : ManualBuyObject
    {
        [SerializeField] private Transform _scaleObject;
        private void Awake()
        {
            onBuy.Once(Show);
        }
        
        public override void DeactivateInternal()
        {
            base.DeactivateInternal();
            _scaleObject.gameObject.SetActive(false);
        }

        public override void SetInBuyModeInternal(bool isToUseSaves)
        {
            base.SetInBuyModeInternal(isToUseSaves);
            _scaleObject.gameObject.SetActive(false);
        }
        
        private void Show()
        {
            _scaleObject.gameObject.SetActive(true);
            _scaleObject.localScale = Vector3.one * 0.01f;
            _scaleObject.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
        }
    }
}