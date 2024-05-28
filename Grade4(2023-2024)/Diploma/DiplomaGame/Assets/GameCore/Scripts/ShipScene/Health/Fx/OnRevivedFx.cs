using Creatives.Scripts;
using GameCore.ShipScene.Damage;
using UnityEngine;

namespace GameCore.ShipScene
{
    public class OnRevivedFx : LiveListener
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private MaterialBlinker _materialBlinker;
        
        protected override void OnDied()
        {
            _particleSystem.Stop();
            _materialBlinker.StopEffect();
        }

        protected override void OnRevived()
        {
            _particleSystem.Play();
            _materialBlinker.StartEffect();
        }
    }
}