using System.Collections.Generic;
using DG.Tweening;
using GameCore.ShipScene.Damage;
using UnityEngine;

namespace GameCore.ShipScene
{
    public class OnDiedUIHider : LiveListener
    {
        [SerializeField] private List<CanvasGroup> _canvasGroups;
        [SerializeField] private float _fadeDuration;
        
        protected override void OnDied()
        {
            DOTween.Kill(this);
            foreach (var canvasGroup in _canvasGroups)
                canvasGroup.DOFade(0, _fadeDuration).SetId(this);
        }

        protected override void OnRevived()
        {
            DOTween.Kill(this);
            foreach (var canvasGroup in _canvasGroups)
                canvasGroup.DOFade(1, _fadeDuration).SetId(this);
        }
    }
}