using GameBasicsCore.Game.API.Ads;

#pragma warning disable 0649
namespace GameCore.Common.Controllers
{
    public class AdsEnableController : IAdsEnableController
    {
        public bool IsBannerEnable() => true;
        public bool IsInterstitialEnable() => true;
        public bool IsRvEnable() => true;
        public bool IsInPlayEnable() => false;
    }
}