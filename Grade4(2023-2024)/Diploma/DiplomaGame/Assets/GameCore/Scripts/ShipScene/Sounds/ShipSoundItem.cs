using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.ShipScene.Sounds
{
    public class ShipSoundItem : MonoBehaviour, IPoolItem<ShipSoundItem>
    {
        [SerializeField] private string _id;
        [SerializeField] private AudioSource _audioSource;

        public string id => _id;
        public AudioSource source => _audioSource;
        public IPool<ShipSoundItem> pool { get; set; }
        public bool isTook { get; set; }

        private void OnDisable()
        {
            if(pool != null)
                pool.Return(this);
        }

        public void Play()
        {
            _audioSource.Play();
            if (pool == null)
            {
                TakeItem();
                return;
            }
            float duration = source.clip.length;
            DOVirtual.DelayedCall(duration, () => pool.Return(this));
        }
        
        public void TakeItem()
        {
            _audioSource.Stop();
            _audioSource.mute = false;
        }

        public void ReturnItem()
        {
            _audioSource.Stop();
            _audioSource.mute = true;
        }
    }
}