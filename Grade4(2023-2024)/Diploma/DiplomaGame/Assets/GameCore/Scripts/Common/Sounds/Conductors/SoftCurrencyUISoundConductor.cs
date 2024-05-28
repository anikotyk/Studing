using System;
using GameCore.Common.Sounds.Api;
using GameBasicsCore.Game.Sounds;
using GameBasicsCore.Game.Sounds.Conductors;
using UnityEngine;

namespace GameCore.Common.Sounds.Conductors
{
    public class SoftCurrencyUISoundConductor : MultiplePoolSoundConductor, ISoftCurrencyUISfxPlayer
    {
        [SerializeField] private SoundItem _sndCollected;
        [SerializeField] private SoundItem _sndSpawned;

        private float _minInterval = 0.1f;
        private DateTime _lastPlayedDateTime;
        
        public void PlayCollected()
        {
            if ((DateTime.Now - _lastPlayedDateTime).TotalSeconds < _minInterval) return;
            _lastPlayedDateTime = DateTime.Now;
            Spawn(_sndCollected).Play();
        }

        public void PlaySpawned()
        {
            if ((DateTime.Now - _lastPlayedDateTime).TotalSeconds < _minInterval) return;
            _lastPlayedDateTime = DateTime.Now;
            Spawn(_sndSpawned).Play();
        }
    }
}