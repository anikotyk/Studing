using JetBrains.Annotations;
using GameBasicsCore.Game.API.Ads;
using GameBasicsCore.Game.Controllers.AppMetrics;
using GameBasicsCore.Game.Controllers.AppMetrics.Playtime;
using GameBasicsCore.Game.SaveProperties.SaveData;
using UnityEngine;
using Zenject;

#pragma warning disable 0649
namespace GameCore.Common.Controllers
{
    public class InterstitialAdCooldownController : IInterstitialAdCooldownController
    {
        [Inject, UsedImplicitly] public AppPlaytimeController appPlaytimeController { get; }
        [Inject, UsedImplicitly] public AppRunDateTimeController appRunDateTimeController { get; }
        [Inject, UsedImplicitly] public AppMetricsSaveData appMetricsSaveData { get; }

        public bool isToResetCooldownAfterRv => true;
        
        public int FirstInterstitialSeconds => 300;
        public int TimerInterstitialDefault => 90;
        public int RvPlusTimerInterstitialValue => 30;

        public int GetCooldown()
        {
            if (appPlaytimeController.totalPlaytime < FirstInterstitialSeconds)
            {
                int time = (int) (FirstInterstitialSeconds - appPlaytimeController.totalPlaytime);
                return Mathf.Max(time, TimerInterstitialDefault);
            }

            return TimerInterstitialDefault;
        }

        public int GetCooldownAfterRv()
        {
            return GetCooldown();
        }
        
        public int GetIncreaseCooldownValueAfterRv()
        {
            return RvPlusTimerInterstitialValue;
        }
    }
}