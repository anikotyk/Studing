using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Settings.GameCore;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace GameCore.Common.Controllers
{
    public class AudioController : InSceneController
    {
        [Inject, UsedImplicitly] public AudioMixer mixer { get; }
        [Inject, UsedImplicitly] public UserSettingsController userSettingsController { get; }

        public override void Construct()
        {
            var settings = GameCoreNewSettings.def.userSettings;
            
            if (settings.availableMusic.value)
            {
                hub.Get<NCSgnl.UserSettingsSignals.SwitchMusic>().On(value => SetMusic());
            }
            if (settings.availableSounds.value)
            {
                hub.Get<NCSgnl.UserSettingsSignals.SwitchSounds>().On(value => SetSound());
            }
            
            SetMusic();
            SetSound();
        }

        private void SetMusic()
        {
            float value = userSettingsController.musicEnabledSaveProperty.value ? 1f : 0f;
            mixer.SetFloat("MusicVolume", LinearToDecibel(value));
        }

        private void SetSound()
        {
            float value = userSettingsController.soundsEnabledSaveProperty.value ? 1f : 0f;
            mixer.SetFloat("SoundsVolume", LinearToDecibel(value));
        }
        
        private float LinearToDecibel(float linear)
        {
            // If the linear value is 0, return the minimum decibel value
            if (linear <= 0.0001f)
            {
                return -80.0f; // Approximate minimum value for AudioMixer in Unity
            }

            // Convert linear value to decibels
            return 20.0f * Mathf.Log10(linear);
        }
    }
}