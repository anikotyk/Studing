using UnityEngine;

namespace GameCore.ShipScene.Damage
{
    public class DamageFx : DamageListener
    {
        [SerializeField] private ParticleSystem _damageParticle;
        
        protected override void OnDamaged(int damage)
        {
            _damageParticle.Play();    
        }
    }
}