using GameCore.Common.Sounds.Api;
using GameBasicsCore.Game.Sounds;
using GameBasicsCore.Game.Sounds.Conductors;
using UnityEngine;

namespace GameCore.Common.Sounds.Conductors
{
    public class UpgradeUnitSoundConductor : MultiplePoolSoundConductor, IUpgradeUnitSfxPlayer
    {
        [SerializeField] private SoundItem _sndUpgrade;
        
        public void Play()
        {
            Spawn(_sndUpgrade).Play();
        }
    }
}