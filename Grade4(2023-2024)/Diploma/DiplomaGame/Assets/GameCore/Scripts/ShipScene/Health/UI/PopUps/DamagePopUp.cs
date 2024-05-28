using System;
using DG.Tweening;
using GameBasicsCore.Game.Views.UI.PopUps;
using TMPro;
using UnityEngine;

namespace GameCore.ShipScene.PopUps
{
    public class DamagePopUp : MonoBehaviour, IPoolItem<DamagePopUp>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeOutDuration;
        
        public IPool<DamagePopUp> pool { get; set; }
        public bool isTook { get; set; }

        public void SetDamage(int damage)
        {
            _text.text = $"-{damage}";
        }

        public void Hide()
        {
            _canvasGroup.DOFade(0, _fadeOutDuration).SetId(this).OnComplete(() =>
            {
                pool.Return(this);
            });
        }

        public void TakeItem()
        {
            DOTween.Kill(this);
        }

        public void ReturnItem()
        {
        }
    }
}