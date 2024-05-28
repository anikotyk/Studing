using System.Collections.Generic;
using GameCore.ShipScene.Damage;
using GameCore.ShipScene.Extentions;
using NaughtyAttributes;
using GameBasicsCore.Game.Misc;
using Plugins.StaserSDK.Animations;
using UnityEngine;

namespace GameCore.ShipScene
{
    public class OnDiedFx : LiveListener
    {
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private List<AnimatorParameterApplier> _animations;
        [SerializeField] private List<ReversibleAnimatorApplier> _reversibleAnimations;

        protected override void OnDied()
        {
            ApplyDefault();
        }

        protected override void OnRevived()
        {
            ApplyRevert();
        }

        private void ApplyDefault()
        {
            foreach (var reversibleAnimation in _reversibleAnimations)
                reversibleAnimation.Apply();

            foreach (var animation in _animations)
                animation.Apply();

            if(_particle != null)
                _particle.Play();
        }

        private void ApplyRevert()
        {
            foreach (var reversibleAnimation in _reversibleAnimations)
                reversibleAnimation.Revert();

            if(_particle != null)
                _particle.Stop();
        }
    }
}