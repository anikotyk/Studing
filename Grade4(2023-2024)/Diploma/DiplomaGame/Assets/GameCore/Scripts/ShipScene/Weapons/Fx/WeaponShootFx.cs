using System;
using UnityEngine;

namespace GameCore.ShipScene.Weapons.Fx
{
    public class WeaponShootFx : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private ParticleSystem _particle;

        private void OnEnable()
        {
            _weapon.shot.On(OnShot);
        }

        private void OnDisable()
        {
            _weapon.shot.Off(OnShot);
        }

        private void OnShot()
        {
            if(_particle == null)
                return;
            _particle.Play();
        }
    }
}