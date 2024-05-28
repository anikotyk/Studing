using GameCore.ShipScene.Sounds;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Weapons.Fx
{
    public class WeaponShootSound : WeaponShotListener
    {
        [SerializeField] private ShipSoundItem _shipSoundItem;
        
        [Inject, UsedImplicitly] public ShipSoundsManager shipSoundsManager { get; }
        
        public override void OnShoot()
        {
            shipSoundsManager.Play(_shipSoundItem);
        }
    }
}