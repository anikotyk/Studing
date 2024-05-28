using GameCore.Common.Controllers;
using NaughtyAttributes;
using GameBasicsCore.Tools.Extensions;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

#pragma warning disable 0649
namespace GameCore.Common.Installers.SceneContext
{
    public class AudioInstaller : MonoInstaller
    {
        [InfoBox("AudioMixer should be injected through installer, because using Addressables will cause duplicates in build assets.")]
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AudioMixerGroup _musicMixerGroup;
        [SerializeField] private AudioMixerGroup _soundsMixerGroup;

        public override void InstallBindings()
        {
            Container.BindInstance(_mixer).WhenInjectedInto<AudioController>();
            Container.BindInstance(_musicMixerGroup).WhenInjectedInto<AudioController>();
            Container.BindInstance(_soundsMixerGroup).WhenInjectedInto<AudioController>();

            Container.BindDefault<AudioController>();
        }
    }
}