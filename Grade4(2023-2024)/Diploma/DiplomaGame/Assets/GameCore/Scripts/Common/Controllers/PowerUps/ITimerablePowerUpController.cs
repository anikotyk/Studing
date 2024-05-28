using System;
using GameBasicsSignals;

namespace GameCore.Common.Controllers.PowerUps
{
    public interface ITimerablePowerUpController
    {
        public TimeSpan timeUntilSuperPowerTurnOff { get; }
        
        public TheSignal onSuperPowerEffectStart { get; }
        public TheSignal onSuperPowerEffectEnd { get; }
        public TheSignal<float> onSuperPowerWaitEffectEndUpdate { get; }
        
        public TheSignal onSuperPowerPause { get; }
        public TheSignal onSuperPowerUnpause { get; }
    }
}