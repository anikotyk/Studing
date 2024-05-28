using GameCore.Common.Misc;
using GameCore.GameScene_Iceland.LevelItems.Managers;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Items
{
    public class SwimHuntingPlatform : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        [Inject, UsedImplicitly] public SwimHuntingManager swimHuntingManager { get; }
        
        [SerializeField] private StartHuntingDonatePlatform _startHuntingDonatePlatform;

        public override void Construct()
        {
            base.Construct();

            _startHuntingDonatePlatform.onCompletedDonation.On(OnDonatingResourcesComplete);
            hub.Get<GCSgnl.SwimHuntingSignals.Ended>().On(() =>
            {
                _startHuntingDonatePlatform.Activate();
            });
        }

        private void OnDonatingResourcesComplete()
        {
            swimHuntingManager.StartHunting();
        }
    }
}