using System;
using System.Collections.Generic;
using GameBasicsSignals;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.ShipScene.Weapons
{
    public class WeaponLinker : MonoBehaviour
    {
        [SerializeField] private List<Weapon> _weapons;
        [SerializeField] private WeaponUpgradesManager _weaponUpgradesManager;
        public Weapon weapon => _weapons[_weaponUpgradesManager.currentWeaponIndex-1];
        public  List<Weapon> weapons => _weapons;

        private void OnEnable()
        {
            foreach (var gun in _weapons)
            {
                gun.gameObject.SetActive(false);                
            }
            weapon.gameObject.SetActive(true);
        }
    }
}