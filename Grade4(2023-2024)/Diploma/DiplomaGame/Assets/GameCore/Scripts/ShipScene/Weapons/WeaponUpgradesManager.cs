using GameCore.ShipScene.DataConfigs;
using JetBrains.Annotations;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Models;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Weapons
{
    public class WeaponUpgradesManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public UpgradesController upgradesController { get; }
        [SerializeField] private UpgradePropertyDataConfig _weaponUpgradeConfig;
        [SerializeField] private WeaponsGroupDataConfig _weaponsGroupDataConfig;
        
        private UpgradePropertyModel _weaponUpgradeModel;
        public int currentWeaponIndex => _weaponUpgradeModel?.level ?? 1;
        public WeaponDataConfig currentWeaponConfig => _weaponsGroupDataConfig[currentWeaponIndex];
        
        public readonly TheSignal onChange = new ();
        
        public override void Construct()
        {
            _weaponUpgradeModel = upgradesController.GetModel(_weaponUpgradeConfig);
            _weaponUpgradeModel.onChange.On(_ =>
            {
                onChange.Dispatch();
            });
        }
    }
}