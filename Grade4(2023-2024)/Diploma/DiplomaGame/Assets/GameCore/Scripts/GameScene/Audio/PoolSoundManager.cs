using System.Collections.Generic;
using System.Linq;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene.Audio
{
    public class PoolSoundManager : InjCoreMonoBehaviour
    {
        [SerializeField] private AudioSource _sound;
        private List<AudioSource> _sounds = new List<AudioSource>();

        public void PlaySound()
        {
            var sound = _sounds.FirstOrDefault(sound => !sound.isPlaying);
            if (sound == null)
            {
                sound = Instantiate(_sound, transform);
                _sounds.Add(sound);
            }
            
            sound.Play();
        }
    }
}