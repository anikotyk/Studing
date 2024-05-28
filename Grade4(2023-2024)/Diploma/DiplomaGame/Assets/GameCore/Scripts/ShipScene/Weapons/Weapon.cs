using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.ShipScene.Extentions;
using GameCore.ShipScene.Weapons.Bullets;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSignals;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameCore.ShipScene.Weapons
{
    [RequireComponent(typeof(Collider))]
    public class Weapon : InjCoreMonoBehaviour
    {
        [SerializeField] private float _reloadDuration;
        [SerializeField] private float _shootDelay;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _poolSize = 10;
        [SerializeField] private Transform _poolParent;
        [SerializeField] private ReversibleAnimatorApplier _handleAnimation;

        private SimplePool<BulletBase> _bulletsPool;
        private List<Health> _enemyInQueue = new();

        private float _startRotateSpeed = 10f;
        
        private bool _shootingStarted = false;

        private Collider _colliderCached;
        public Collider collider
        {
            get
            {
                if (_colliderCached == null)
                    _colliderCached = GetComponent<Collider>();
                return _colliderCached;
            }
        }

        public ReversibleAnimatorApplier handleAnimation => _handleAnimation;
        
        public TheSignal shot { get; } = new();
        public TheSignal<Transform> shootingStarted { get; } = new();
        public TheSignal shootingEnded { get; } = new();
        

        public override void Construct()
        {
            InitPool();
        }

        private void Awake()
        {
            InitPool();
        }

        private void OnDisable()
        {
            _enemyInQueue.Clear();
        }

        private void InitPool()
        {
            if(_bulletsPool is { Initialized: true })
                return;
            _bulletPrefab.transform.localPosition = Vector3.zero;
            _bulletsPool = new SimplePool<BulletBase>(_bulletPrefab, _poolSize, _poolParent);
            _bulletsPool.Initialize();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Health health))
            {
                if(health.CanApplyDamage() == false || _bulletPrefab.TargetTypes.CanApplyDamage(health) == false)
                    return;
                _enemyInQueue.Add(health);
                Shoot();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Health health))
            {
                _enemyInQueue.Remove(health);
            }
        }

        private void Shoot()
        {
            if(DOTween.IsTweening(this))
                return;
            ClearDeadEnemies();
            if (CanShoot() == false)
            {
                shootingEnded.Dispatch();
                return;
            }

           
            var target = GetShootTarget();
            shootingStarted.Dispatch(target);
            
            DOVirtual.DelayedCall(_shootDelay, () =>
            {
                ShootBullet(target);
                shot.Dispatch();
            }).SetId(this);
            
            DOVirtual.DelayedCall(_shootDelay + _reloadDuration, Shoot).SetId(this);
        }

        private void ClearDeadEnemies()
        {
            _enemyInQueue.RemoveAll(x => x.CanApplyDamage() == false);
        }
        
        private bool CanShoot()
        {
            return _enemyInQueue.Count > 0 &&  _bulletsPool != null;
        }
        
        private Transform GetShootTarget()
        {
            var enemies = _enemyInQueue.OrderBy(health => VectorExtensions.SqrDistance(health.transform, transform)).ToList();
            return enemies[0].transform;
        }
        
        private void ShootBullet(Transform target)
        {
            var bullet = _bulletsPool.Get();
            bullet.ShootToTarget(target);
            bullet.endedLifetime.Once(_ =>
            {
                bullet.pool.Return(bullet);
            });
        }
        
        

       
    }
}