using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Sounds;
using GameBasicsCore.Game.Sounds.Conductors;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Sounds.Conductors
{
    public class UIWindowsSoundConductor : MultiplePoolSoundConductor
    {
        [SerializeField] private SoundItem _sndWindowOpen;
        [SerializeField] private SoundItem _sndWindowClose;
        
        [Inject, UsedImplicitly] public SignalHub hub { get; set; }
        
        public override void Construct()
        {
            hub.Get<NCSgnl.IUIWindowSignals.ShowStart>().On(window =>
            {
                if (window is UIDialogWindow dialog)
                {
                    if (dialog.playSoundOnShowAndHide) Spawn(_sndWindowOpen).Play();
                }
            });
            hub.Get<NCSgnl.IUIWindowSignals.HideStart>().On(window =>
            {
                if (window is UIDialogWindow dialog)
                {
                    if (dialog.playSoundOnShowAndHide) Spawn(_sndWindowClose).Play();
                }
            });
        }
    }
}