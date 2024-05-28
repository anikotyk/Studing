using GameCore.ShipScene.Common;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.ShipScene.Weapons
{
    public class WeaponIsShootingListener : InjCheckCoreMonoBehaviour
    {
        [SerializeField] private WeaponLinker _weaponLinker;

        private Weapon currentWeapon => _weaponLinker.weapon;

        public bool isShooting { get; private set; } = false;
        
        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            if(currentWeapon == null)
                return;
            currentWeapon.shootingStarted.On(ShootingStarted);
            currentWeapon.shootingEnded.On(ShootingEnded);
        }
        
        private void OnDisable()
        {
            Unsubscribe();
            ShootingEnded();
        }

        private void Unsubscribe()
        {
            if(currentWeapon == null)
                return;
            currentWeapon.shootingStarted.Off(ShootingStarted);
            currentWeapon.shootingEnded.Off(ShootingEnded);
        }

        private void ShootingStarted(Transform target)
        {
            isShooting = true;
            OnShootingStarted(target);
        }

        protected virtual void OnShootingStarted(Transform target){}

        private void ShootingEnded()
        {
            isShooting = false;
            OnShootingEnded();
        }
        
        protected virtual void OnShootingEnded(){}
    }
}