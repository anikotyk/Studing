using GameCore.Common.LevelItems;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Controllers.Ads.Single;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class DelayAdsManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [InjectOptional, UsedImplicitly] public InterstitialAdUnitController interstitialAdUnitController { get; }
        [InjectOptional, UsedImplicitly] public BannerAdUnitController bannerAdUnitController { get; }
        
        [SerializeField] private BuyObject _startInterstitialsBuyObject;
        [SerializeField] private BuyObject _startBannerBuyObject;

        public override void Construct()
        {
            base.Construct();
            initializeInOrderController.Add(Initialize, 5000);
        }
        
        private void Initialize()
        {
            if (interstitialAdUnitController != null)
            {
                if(_startInterstitialsBuyObject && !_startInterstitialsBuyObject.isBought)
                {
                    interstitialAdUnitController.Pause(this.name);
                    _startInterstitialsBuyObject.onBuy.Once(() =>
                    {
                        interstitialAdUnitController.Unpause(this.name);
                    });
                }
            }

            if (bannerAdUnitController != null)
            {
                if(_startBannerBuyObject && !_startBannerBuyObject.isBought)
                {
                    bannerAdUnitController.runner.Hide();
                    _startBannerBuyObject.onBuy.Once(() =>
                    {
                        bannerAdUnitController.runner.Show();
                    });
                }
            }
        }
    }
}