using JetBrains.Annotations;
using GameBasicsCore.Game.API.Ads;
using GameBasicsCore.Game.Controllers.AppMetrics;
using GameBasicsCore.Game.Controllers.AppMetrics.Playtime;
using GameBasicsCore.Game.SaveProperties.SaveData;
using Zenject;

#pragma warning disable 0649
namespace GameCore.Common.Controllers
{
    public class BannerAdCooldownController : IBannerAdCooldownController
    {
        [Inject, UsedImplicitly] public AppPlaytimeController appPlaytimeController { get; }
        [Inject, UsedImplicitly] public AppRunDateTimeController appRunDateTimeController { get; }
        [Inject, UsedImplicitly] public AppMetricsSaveData appMetricsSaveData { get; }
        
        public int FirstBannerSessionSeconds => 5; // seconds before first banner can be shown in each session (if allready first_banner_seconds criteria is met)
        public int FirstBannerSeconds => 400; // Seconds before 1st BN is shown
        
        public int GetCooldown()
        {
            if (appPlaytimeController.totalPlaytime < FirstBannerSeconds)
            {
                int time = (int) (FirstBannerSeconds - appPlaytimeController.totalPlaytime);
                if (time < 0) time = 0;
                return time;
            }

            return FirstBannerSessionSeconds;
        }
    }
}