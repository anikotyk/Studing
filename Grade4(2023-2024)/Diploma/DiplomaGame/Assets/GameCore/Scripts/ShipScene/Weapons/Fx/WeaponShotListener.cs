using System;
using UnityEngine;

namespace GameCore.ShipScene.Weapons.Fx
{
    public abstract class WeaponShotListener : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;

        private void OnEnable()
        {
            _weapon.shot.On(OnShoot);
        }

        private void OnDisable()
        {
            _weapon.shot.Off(OnShoot);
        }

        public abstract void OnShoot();

    }
}