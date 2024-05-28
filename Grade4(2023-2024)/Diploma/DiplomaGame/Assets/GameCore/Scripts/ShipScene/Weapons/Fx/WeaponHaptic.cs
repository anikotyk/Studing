using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCoreModules.GameBasicsCore.Module.Haptic;
using Zenject;

namespace GameCore.ShipScene.Weapons.Fx
{
    public class WeaponHaptic : WeaponShotListener
    {
        [Inject, UsedImplicitly] public IHapticService hapticService { get; }
        
        public override void OnShoot()
        {
            hapticService.Selection();
        }
    }
}