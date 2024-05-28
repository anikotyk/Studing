using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.ShipScene.Weapons.Bullets.Projectiles
{
    public class BulletProjectileFx : MonoBehaviour
    {
        [SerializeField] private BulletProjectile _bulletProjectile;
        [SerializeField] private Transform _particleParent;
        [SerializeField] private ParticleSystem _hitParticle;
        [SerializeField] private ParticleSystem _destroyParticle;
        
        private void OnEnable()
        {
            _bulletProjectile.used.On(OnUsed);
            _bulletProjectile.hit.On(OnHit);
            _bulletProjectile.transform.localScale = Vector3.one;
            ResetParticles();
        }
        
        private void OnDisable()
        {
            _bulletProjectile.used.Off(OnUsed);
            _bulletProjectile.hit.Off(OnHit);
        }

        private void ResetParticles()
        {
            ResetParticle(_hitParticle);
            ResetParticle(_destroyParticle);
        }

        private void ResetParticle(ParticleSystem particle)
        {
            if(particle == null)
                return;
            particle.Stop();
            particle.gameObject.SetActive(false);

            var particleSystemTransform = particle.transform;
            particleSystemTransform.SetParent(_particleParent);
            particleSystemTransform.localPosition = Vector3.zero;
        }
        
        private void OnHit()
        {
            SpawnParticle(_hitParticle);
        }
        
        private void OnUsed(BulletProjectile _)
        {
            SpawnParticle(_destroyParticle);
           _bulletProjectile.gameObject.SetActive(false);
        }

        private void SpawnParticle(ParticleSystem particle)
        {
            if(particle == null)
                return;
            particle.gameObject.SetActive(true);
            particle.transform.SetParent(null);
            particle.Play();
            DOVirtual.DelayedCall(particle.main.duration, () =>
            {
                particle.transform.SetParent(_particleParent, true);
                particle.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}