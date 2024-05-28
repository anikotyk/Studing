using System.Collections;
using System.Collections.Generic;
using GameBasicsCore.Game.Contexts;
using GameBasicsCore.Game.LevelSystem;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.Misc
{
    public class AudioStarter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;
        private void Awake()
        {
            StartCoroutine(StartAudioCoroutine());
        }

        private IEnumerator StartAudioCoroutine()
        {
            float volume = _audio.volume;
            float currentVolume = 0;
            float speed = 5;
            _audio.volume = currentVolume;
            _audio.Play();
            while (currentVolume < volume)
            {
                currentVolume += Time.deltaTime * speed;
                yield return null;
                _audio.volume = currentVolume;
            }
        }
    }

}