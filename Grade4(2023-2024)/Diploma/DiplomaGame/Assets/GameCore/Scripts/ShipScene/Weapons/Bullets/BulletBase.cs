using GameCore.ShipScene.Weapons.Bullets.Projectiles;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.ShipScene.Weapons.Bullets
{
    public abstract class BulletBase : MonoBehaviour, IPoolItem<BulletBase>
    {
        [SerializeField] private HealthTargetType[] _targetTypes;
        public IPool<BulletBase> pool { get; set; }
        
        private bool _projectileInitialized = false;
        private BulletProjectile[] _bulletProjectilesCached;
        public BulletProjectile[] projectiles
        {
            get
            {
                if (_projectileInitialized == false)
                {
                    _bulletProjectilesCached = GetComponentsInChildren<BulletProjectile>();
                    _projectileInitialized = true;
                }
                return _bulletProjectilesCached;
            }
        }

        public HealthTargetType[] TargetTypes => _targetTypes;
        public bool isTook { get; set; }
        public TheSignal<BulletBase> receivedTarget { get; } = new();
        public TheSignal<BulletBase> endedLifetime { get; } = new();

        public void ShootToTarget(Transform target)
        {
            foreach (var projectile in projectiles)
            {
                projectile.targetTypes.AddRange(_targetTypes);
                projectile.ResetProjectile();
                projectile.hit.Once(OnProjectileHit);
            }

            OnShootToTarget(target);
        }

        private void OnProjectileHit()
        {
            foreach (var projectile in projectiles)
                projectile.hit.Off(OnProjectileHit);
            OnHit();
        }

        protected virtual void OnHit(){}
        
        public abstract void OnShootToTarget(Transform target);
        
        public virtual void TakeItem()
        {
        }

        public virtual void ReturnItem()
        {
        }
    }
}