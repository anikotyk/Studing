using GameCore.Common.LevelItems.PowerUps;
using GameBasicsCore.Game.Sounds;
using GameBasicsCore.Game.Sounds.Conductors;
using UnityEngine;

namespace GameCore.Common.Sounds.Conductors
{
    public class PowerUpContainerSoundConductor : MultiplePoolSoundConductor
    {
        [SerializeField] private SoundItem _sndOnEntered;
        [SerializeField] private SoundItem _sndOnExited;
        [SerializeField] private SoundItem _sndClaim;
        
        private PowerUpContainer _containerCached;
        public PowerUpContainer container => _containerCached ??= GetComponentInParent<PowerUpContainer>(true);

        private void Start()
        {
            container.onCharacterEntered.On(OnCharacterEntered);
            container.onCharacterExited.On(OnCharacterExited);
            container.onClaimed.On(OnClaimed);
        }

        private void OnCharacterEntered()
        {
            if (_sndOnEntered) Spawn(_sndOnEntered).Play();
        }
        
        private void OnCharacterExited()
        {
            if (_sndOnExited) Spawn(_sndOnExited).Play();
        }
        
        private void OnClaimed()
        {
            if (_sndClaim) _sndClaim.Play();
        }
    }
}
