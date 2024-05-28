using System.Collections;
using DG.Tweening;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Extensions;
using UnityEngine;

namespace GameCore.ShipScene.Weapons.Bullets
{
    public class Bullet : BulletBase
    {
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private float _projectileRotateSpeed;
        [SerializeField] private float _minDistanceToLookAt = 0.1f;
        [SerializeField] private float _maxDistance;
        
        private bool _isHit = false;
        private Coroutine _shootRoutine;

        public override void OnShootToTarget(Transform target)
        {
            _isHit = false;
            transform.LookAt(target);
            _shootRoutine = StartCoroutine(Shoot(target));
            OnShootStart(target);
        }

        protected override void OnHit()
        {
            _isHit = true;
        }

        private bool IsTargetForward(Vector3 targetPosition)
        {
            var currentTransform = transform;
            return Vector3.Dot(currentTransform.forward, targetPosition - currentTransform.position) < -0.15f;
        }
        private IEnumerator Shoot(Transform target)
        {
            Vector3 startPosition = transform.position;
            float speed = Time.fixedDeltaTime * _projectileSpeed;
            while (_isHit == false && IsTargetForward(target.position) && target.gameObject.activeInHierarchy)
            {
                if (VectorExtensions.SqrDistance(target.position, transform.position) >
                    _minDistanceToLookAt * _minDistanceToLookAt)
                {
                    Vector3 relativePos = target.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _projectileRotateSpeed * Time.fixedDeltaTime);
                }
                
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
                yield return new WaitForFixedUpdate();
            }
            
            receivedTarget.Dispatch(this);
            
            while (VectorExtensions.SqrDistance(startPosition, transform.position) <= _maxDistance * _maxDistance)
            {
                var currentTransform = transform;
                currentTransform.position += currentTransform.forward * speed;
                yield return new WaitForFixedUpdate();
            }
            ShootEnd();
        }
        
        protected virtual void OnShootStart(Transform target){}
        
        private void ShootEnd()
        {
            if(_shootRoutine != null)
                StopCoroutine(_shootRoutine);
            _shootRoutine = null;
            OnShootEnd();
            UseBullets();
        }
        
        protected virtual void OnShootEnd(){}
        
        private void UseBullets()
        {
            foreach (var projectile in projectiles)
                projectile.ForceUse();
            endedLifetime.Dispatch(this);
        }

        public override void TakeItem()
        {
            transform.SetParent(null);
            DOTween.Kill(this);
        }

        public override void ReturnItem()
        {
            DOTween.Kill(this);
        }
    }
}