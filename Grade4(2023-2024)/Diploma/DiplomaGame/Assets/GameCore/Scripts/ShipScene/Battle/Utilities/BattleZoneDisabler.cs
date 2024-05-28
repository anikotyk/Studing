using DG.Tweening;
using GameCore.ShipScene.Battle.Waves;
using UnityEngine;

namespace GameCore.ShipScene.Battle.Utilities
{
    public class BattleZoneDisabler : BattleStartEndListener
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _zoomTarget;
        [SerializeField] private float _zoomDuration;
        
        protected override void OnBattleStarted(Wave wave)
        {
            DOTween.Kill(this);
            _collider.enabled = false;
            _zoomTarget.DOScale(0, _zoomDuration).SetEase(Ease.InBack).SetId(this);
        }

        protected override void OnBattleEnded(Wave wave)
        {
            DOTween.Kill(this);
            _collider.enabled = true;
            _zoomTarget.DOScale(1, _zoomDuration).SetEase(Ease.OutBack).SetId(this);
        }
    }
}