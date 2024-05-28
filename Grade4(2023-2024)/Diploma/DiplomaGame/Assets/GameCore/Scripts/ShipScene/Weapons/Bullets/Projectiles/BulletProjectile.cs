using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.ShipScene.Weapons.Bullets.Projectiles
{
    [RequireComponent(typeof(Collider))]
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private bool _limitedAttachedTargets = true;
        [SerializeField, ShowIf(nameof(_limitedAttachedTargets))] private int _maxTargets;

        private int _usedCount = 0;
        private bool _alreadyUsed = false;

        public List<HealthTargetType> targetTypes { get; private set; } = new();

        private bool _colliderFounded = false;
        private Collider _colliderCached;
        private Collider _collider
        {
            get
            {
                if (_colliderFounded == false)
                {
                    _colliderCached = GetComponent<Collider>();
                    _colliderFounded = true;
                }

                return _colliderCached;
            }
        }

        public TheSignal<BulletProjectile> used { get; } = new();
        public TheSignal hit { get; } = new();

        public void ResetProjectile()
        {
            _alreadyUsed = false;
            _usedCount = 0;
            _collider.enabled = true;
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Health health))
            {
                if(targetTypes.Count !=0 && targetTypes.Has(x => x.CanApplyDamage(health) == false))
                    return;
                
                if (health.CanApplyDamage(_damage) == false)
                    return;
            
                health.Damage(_damage);
                hit.Dispatch();
                UseProjectile();
            }
        }

        private void UseProjectile()
        {
            _usedCount++;
            if(_alreadyUsed)
                return;
            if(_limitedAttachedTargets == false)
                return;
            if (_usedCount >= _maxTargets)
            {
                ForceUse();
            }
        }

        public void ForceUse()
        {
            if(_alreadyUsed)
                return;
            _alreadyUsed = true;
            _collider.enabled = false;
            OnUsedProjective();
            used.Dispatch(this);
        }

        protected virtual void OnUsedProjective(){}
    }
}


