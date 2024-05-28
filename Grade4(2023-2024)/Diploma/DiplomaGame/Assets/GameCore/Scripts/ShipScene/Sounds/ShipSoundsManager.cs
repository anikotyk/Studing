using System.Collections.Generic;
using UnityEngine;

namespace GameCore.ShipScene.Sounds
{
    public class ShipSoundsManager : MonoBehaviour
    {
        [SerializeField] private Transform _soundsParent;

        private List<SimplePool<ShipSoundItem>> _soundsPool = new();

        public void Play(ShipSoundItem soundItem)
        {
            SimplePool<ShipSoundItem> pool = _soundsPool.Find(x => x.prefab.id == soundItem.id);
            if (pool == null)
            {
                pool = new SimplePool<ShipSoundItem>(soundItem, 1, _soundsParent);
                pool.Initialize();
                _soundsPool.Add(pool);
            }

            var sound = pool.Get();
            sound.Play();
        }
    }
}