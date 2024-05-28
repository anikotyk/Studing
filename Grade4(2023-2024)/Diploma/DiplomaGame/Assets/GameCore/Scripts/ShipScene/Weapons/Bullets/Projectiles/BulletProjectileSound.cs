using System;
using GameCore.ShipScene.Sounds;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Weapons.Bullets.Projectiles
{
    public class BulletProjectileSound : MonoBehaviour
    {
        [SerializeField] private BulletProjectile _bulletProjectile;
        [SerializeField] private ShipSoundItem _shipSoundItem;
        
        private void OnEnable()
        {
            _bulletProjectile.hit.Once(OnHit);
        }

        private void OnDisable()
        {
            _bulletProjectile.hit.Off(OnHit);
        }

        private void OnHit()
        {
            _shipSoundItem.Play();
        }
    }
}