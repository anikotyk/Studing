using System;
using Creatives.Scripts;
using UnityEngine;

namespace GameCore.ShipScene.Damage
{
    public class DamageBlinker : DamageListener
    {
        [SerializeField] private MaterialBlinker _materialBlinker;
        
        protected override void OnDamaged(int damage)
        {
            _materialBlinker.StartEffect();
        }
    }
}