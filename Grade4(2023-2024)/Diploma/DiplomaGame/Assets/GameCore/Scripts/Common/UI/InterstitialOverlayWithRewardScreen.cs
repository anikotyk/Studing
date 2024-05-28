using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.Common.Sounds.Api;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers.Ads.Single;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Models;
using GameBasicsCore.Game.Models.GameCurrencies;
using GameBasicsCore.Game.Presenters;
using GameBasicsCore.Game.Presenters.Currencies;
using GameBasicsCore.Game.Views.UI.Controls.DataPanels;
using GameBasicsCore.Game.Views.UI.Windows.OverlayScreens;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

#pragma warning disable 0649
namespace GameCore.Common.UI
{
    public class InterstitialOverlayWithRewardScreen : InterstitialOverlayScreen
    {
        [InjectOptional, UsedImplicitly] public SoftCurrencyPresenter softCurrencyPresenter { get; }
        [Inject, UsedImplicitly] public PlayerSoftCurrencyCollectModel softCurrencyModel { get; }
        [Inject, UsedImplicitly] public BunchPopUpsMoveAnimationPresenter bunchPopUpsMoveAnimationPresenter { get; }
        [Inject, UsedImplicitly] public InterstitialAdUnitController interstitialAdUnitController { get; } 
        [InjectOptional, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [InjectOptional, UsedImplicitly] public ISoftCurrencyUISfxPlayer softCurrencyUISfxPlayer { get; }
        
        [SerializeField] private IconLabelDataPanel _rewardPanel;
        [SerializeField] private Button _closeBtn;
        
        private int _reward = -1;
        private float _multiplierTaskPayAmount => 0.05f;

        public override void Construct()
        {
            base.Construct();

            if (buyObjectsManager == null)
            {
                _rewardPanel.gameObject.SetActive(false);
            }
            else
            {
                _reward = CalculateReward();
                SetReward(_reward);
            }

            interstitialAdUnitController.onInterstitialEnded.Once(()=>
            {
               CollectReward();
            });
            
            DOVirtual.DelayedCall(3f, () =>
            {
                _closeBtn.gameObject.SetActive(true);
            }, false).SetLink(gameObject);
            _closeBtn.onClick.AddListener(Hide);
        }

        private int CalculateReward()
        {
            int payment = GetPaymentBasedOnCurrentTask();
            if (payment <= 0) payment = GetPaymentBasedOnPreviousTasks();
            if (payment <= 0) payment = GetPaymentBasedOnNextTasks();
            if (payment <= 0) payment = 5;

            if (payment % 5 > 0)
            {
                payment += 5 - (payment % 5);
            }

            return payment;
        }

        private int GetPaymentBasedOnCurrentTask()
        {
            if (buyObjectsManager.currentBuyObject is DonateBuyObject donateBuyObject)
            {
                return (int)(donateBuyObject.buyPlatform.priceSoftCurrency * _multiplierTaskPayAmount);
            }

            return -1;
        }
        
        private int GetPaymentBasedOnPreviousTasks()
        {
            for (int i = buyObjectsManager.activeBuyObjectIndexSaveProperty.value - 1; i >= 0; i--)
            {
                var buyObject = buyObjectsManager.buyObjects[i];
                if (buyObject is DonateBuyObject donateBuyObject)
                {
                    if (donateBuyObject.buyPlatform.priceSoftCurrency > 0)
                    {
                        return (int)(donateBuyObject.buyPlatform.priceSoftCurrency * _multiplierTaskPayAmount);
                    }
                }
            }
           
            return -1;
        }
        
        private int GetPaymentBasedOnNextTasks()
        {
            for (int i = buyObjectsManager.activeBuyObjectIndexSaveProperty.value + 1; 
                 i < buyObjectsManager.buyObjects.Count; i++)
            {
                var buyObject = buyObjectsManager.buyObjects[i];
                if (buyObject is DonateBuyObject donateBuyObject)
                {
                    if (donateBuyObject.buyPlatform.priceSoftCurrency > 0)
                    {
                        return (int)(donateBuyObject.buyPlatform.priceSoftCurrency * _multiplierTaskPayAmount);
                    }
                }
            }
           
            return -1;
        }
        
        private void SetReward(int amount)
        {
            _rewardPanel.SetLabel(amount+"");
        }

        private void CollectReward()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            if(_reward <= 0) return;
            
            int cntParts = _reward;
            int maxCntParts = 30;
            cntParts = cntParts > maxCntParts ? maxCntParts : cntParts;
            int amount = _reward / cntParts;

            var bunchAnimation = bunchPopUpsMoveAnimationPresenter.Spawn(
                NCStr.SoftCurrency,
                cntParts,
                _rewardPanel.icon.transform.position,
                softCurrencyPresenter.collectedDisplay.GetIconPosition());
            
            bunchAnimation.onChange.On(code =>
            {
                if (code == BunchPopUpsAnimationModel.ItemComplete)
                {
                    softCurrencyModel.Earn(amount, "Interstitial", "Interstitial", true);
                    softCurrencyUISfxPlayer?.PlayCollected();
                }
                else if (code == BunchPopUpsAnimationModel.ItemSpawn)
                {
                    softCurrencyUISfxPlayer?.PlaySpawned();
                }
            });
        }
    }
}