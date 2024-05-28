using DG.Tweening;
using GameCore.ShipScene.PopUps;
using UnityEngine;

namespace GameCore.ShipScene.Damage
{
    public class DamagePopupSpawner : DamageListener
    {
        [SerializeField] private MonoPool<DamagePopUp> _popUpsPool;
        [SerializeField] private float _hideDelay;
        [SerializeField] private float _waitFullDamage;

        private int _damage = 0;

        public override void Construct()
        {
            _popUpsPool.Initialize();
        }
        
        protected override void OnDamaged(int damage)
        {
            _damage = Mathf.Min(health.maxHealth, _damage + damage);
            if(DOTween.IsTweening(this))
                return;
            DOVirtual.DelayedCall(_waitFullDamage, () =>
            {
                var popUp = _popUpsPool.Get();
                popUp.SetDamage(_damage);
                _damage = 0;
                DOVirtual.DelayedCall(_hideDelay, popUp.Hide);
            }).SetId(this);
        }
    }
}