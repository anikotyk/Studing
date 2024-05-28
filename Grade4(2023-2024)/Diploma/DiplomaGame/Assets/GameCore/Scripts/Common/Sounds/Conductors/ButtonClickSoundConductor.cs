using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Sounds;
using GameBasicsCore.Game.Sounds.Conductors;
using UnityEngine;

namespace GameCore.Common.Sounds.Conductors
{
    public class ButtonClickSoundConductor : MultiplePoolSoundConductor, IButtonUiClickSfxPlayer
    {
        [SerializeField] private SoundItem _sndClick;
        
        public void Play()
        {
            Spawn(_sndClick).Play();
        }
    }
}